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

using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Arctic.AppSettings.Mappings
{
    internal class AppSettingMapping : ClassMapping<AppSetting>
    {
        public AppSettingMapping()
        {
            Table("AppSettings");
            DynamicUpdate(true);
            Cache(cache => cache.Usage(CacheUsage.ReadWrite));

            OptimisticLock(OptimisticLockMode.None);
            Id(cl => cl.SettingName, id =>
            {
                id.Generator(Generators.Assigned);
                id.Length(128);
            });
            Property(cl => cl.SettingType, prop => {
                prop.Update(false);
            });
            Property(cl => cl.SettingValue, pm => pm.OptimisticLock(true));
            Property(cl => cl.ctime);
            Property(cl => cl.cuser);
            Property(cl => cl.mtime);
            Property(cl => cl.muser);
            Property(cl => cl.Comment);
        }
    }
}
