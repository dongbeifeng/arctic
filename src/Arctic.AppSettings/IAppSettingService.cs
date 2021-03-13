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

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arctic.AppSettings
{
    public interface IAppSettingService
    {
        /// <summary>
        /// 删除程序设置
        /// </summary>
        /// <param name="name">程序设置名称</param>
        /// <returns></returns>
        Task DeleteAsync(string name);

        /// <summary>
        /// 获取所有程序设置
        /// </summary>
        /// <returns></returns>
        Task<List<AppSetting>> GetAllAsync();

        /// <summary>
        /// 获取具有指定名称的程序设置
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        Task<AppSetting> GetAsync(string settingName);

        /// <summary>
        /// 获取布尔值。
        /// </summary>
        /// <param name="name">程序设置名称</param>
        /// <param name="defaultValue">如果程序设置不存在，且 defaultValue 不为 null，那么将使用默认值创建程序设置</param>
        /// <returns></returns>
        Task<bool?> GetBooleanAsync(string name, bool? defaultValue);

        /// <summary>
        /// 获取数字值。
        /// </summary>
        /// <param name="name">程序设置名称</param>
        /// <param name="defaultValue">如果程序设置不存在，且 defaultValue 不为 null，那么将使用默认值创建程序设置</param>
        /// <returns></returns>
        Task<decimal?> GetNumberAsync(string name, decimal? defaultValue);

        /// <summary>
        /// 获取字符串值。
        /// </summary>
        /// <param name="name">程序设置名称</param>
        /// <param name="defaultValue">如果程序设置不存在，且 defaultValue 不为 null，那么将使用默认值创建程序设置</param>
        /// <returns></returns>
        Task<string?> GetStringAsync(string name, string? defaultValue);

        /// <summary>
        /// 设置布尔值。
        /// </summary>
        /// <param name="name">程序设置名称</param>
        /// <param name="value">要设置的值</param>
        /// <returns></returns>
        Task SetBooleanAsync(string name, bool value);

        /// <summary>
        /// 设置程序设置的备注。
        /// </summary>
        /// <param name="name">程序设置名称</param>
        /// <param name="comment">要设置的值</param>
        /// <returns></returns>
        Task SetCommentAsync(string name, string? comment);

        /// <summary>
        /// 设置数字值。
        /// </summary>
        /// <param name="name">程序设置名称</param>
        /// <param name="value">要设置的值</param>
        /// <returns></returns>
        Task SetNumberAsync(string name, decimal value);

        /// <summary>
        /// 设置字符串值。
        /// </summary>
        /// <param name="name">程序设置名称</param>
        /// <param name="value">要设置的值</param>
        /// <returns></returns>
        Task SetStringAsync(string name, string value);
    }
}