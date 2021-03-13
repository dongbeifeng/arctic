// Copyright 2020-2021 王建军
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arctic.AppSettings
{
    public class AppSettingService : IAppSettingService
    {
        ISession _session;

        public AppSettingService(ISession session)
        {
            _session = session;
        }

        public Task<AppSetting> GetAsync(string name)
        {
            name = name.Trim();
            if (name == null)
            {
                throw new ArgumentException(nameof(name));
            }

            return _session.GetAsync<AppSetting>(name);
        }

        internal async Task<AppSetting> SetAsync(string name, string type, string value)
        {
            name = name.Trim();
            value = value.Trim();

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name 不能为 null 或空白字符串。");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("value 不能为 null 或空白字符串。");
            }

            AppSetting setting = await _session.GetAsync<AppSetting>(name, LockMode.Upgrade).ConfigureAwait(false);
            if (setting == null)
            {
                setting = new AppSetting();
                setting.SettingName = name;
                setting.SettingType = type;
                setting.SettingValue = value;
                await _session.SaveAsync(setting).ConfigureAwait(false);
            }
            else
            {
                if (setting.SettingType != type)
                {
                    throw new InvalidOperationException("不能更改设置类型。");
                }
                setting.SettingValue = value;
                await _session.UpdateAsync(setting).ConfigureAwait(false);
            }
            return setting;
        }

        public Task SetNumberAsync(string name, decimal value)
        {
            name = name.Trim();

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name 不能为 null 或空白字符串。");
            }

            return this.SetAsync(name, AppSettingTypes.数字, value.ToString("0.##########"));
        }

        public async Task<decimal?> GetNumberAsync(string name, decimal? defaultValue)
        {
            name = name.Trim();

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name 不能为 null 或空白字符串。");
            }

            var s = await this.GetAsync(name).ConfigureAwait(false);
            if (s == null)
            {
                if (defaultValue == null)
                {
                    return null;
                }
                else
                {
                    await SetNumberAsync(name, defaultValue.Value).ConfigureAwait(false);
                    return defaultValue;
                }
            }
            if (s.SettingType != AppSettingTypes.数字)
            {
                throw new InvalidOperationException($"设置【{name}】的类型不是数字。");
            }

            return decimal.Parse(s.SettingValue);
        }


        public Task SetStringAsync(string name, string value)
        {
            name = name.Trim();
            value = value.Trim();

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name 不能为 null 或空白字符串。");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("value 不能为 null 或空白字符串。");
            }

            return this.SetAsync(name, AppSettingTypes.字符串, value);
        }

        public async Task<string?> GetStringAsync(string name, string? defaultValue)
        {
            name = name.Trim();

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name 不能为 null 或空白字符串。");
            }

            var s = await this.GetAsync(name).ConfigureAwait(false);
            if (s == null)
            {
                if (defaultValue == null)
                {
                    return null;
                }
                else
                {
                    await SetStringAsync(name, defaultValue).ConfigureAwait(false);
                    return defaultValue;
                }
            }
            if (s.SettingType != AppSettingTypes.字符串)
            {
                throw new InvalidOperationException($"；设置【{name}】的类型不是字符串。");
            }

            return s.SettingValue;
        }

        public Task SetBooleanAsync(string name, bool value)
        {
            name = name.Trim();

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name 不能为 null 或空白字符串。");
            }

            return this.SetAsync(name, AppSettingTypes.布尔, value.ToString().ToLower());
        }

        public async Task<bool?> GetBooleanAsync(string name, bool? defaultValue)
        {
            name = name.Trim();

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name 不能为 null 或空白字符串。");
            }

            var s = await this.GetAsync(name).ConfigureAwait(false);
            if (s == null)
            {
                if (defaultValue == null)
                {
                    return null;
                }
                else
                {
                    await SetBooleanAsync(name, defaultValue.Value);
                    return defaultValue.Value;
                }
            }
            if (s.SettingType != AppSettingTypes.布尔)
            {
                throw new InvalidOperationException($"；设置【{name}】的类型不是布尔。");
            }

            return bool.Parse(s.SettingValue);
        }

        public Task<List<AppSetting>> GetAllAsync()
        {
            return _session.Query<AppSetting>()
                .WithOptions(opt => opt.SetCacheable(true))
                .ToListAsync();
        }

        public async Task DeleteAsync(string name)
        {
            name = name.Trim();

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name 不能为 null 或空白字符串。");
            }

            AppSetting setting = await _session.GetAsync<AppSetting>(name, LockMode.Upgrade).ConfigureAwait(false);
            if (setting != null)
            {
                await _session.DeleteAsync(setting);
            }
        }

        public async Task SetCommentAsync(string name, string? comment)
        {
            name = name.Trim();

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name 不能为 null 或空白字符串。");
            }

            AppSetting setting = await _session.GetAsync<AppSetting>(name, LockMode.Upgrade).ConfigureAwait(false);
            if (setting != null)
            {
                setting.Comment = comment;
            }
        }

    }
}
