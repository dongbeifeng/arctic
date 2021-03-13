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

namespace Arctic.AppCodes.Mappings
{
    internal class AppCodeMapping : ClassMapping<AppCode>
    {
        public AppCodeMapping()
        {
            Table("AppCodes");
            DynamicUpdate(true);
            Cache(cache => cache.Usage(CacheUsage.ReadWrite));

            ComposedId(id =>
            {
                id.Property(cl => cl.AppCodeType, prop => prop.Update(false));
                id.Property(cl => cl.AppCodeValue, prop => prop.Update(false));
            });

            Property(cl => cl.Description);
            Property(cl => cl.Visible);
            Property(cl => cl.Comment);
            Property(cl => cl.Scope);

            Property(cl => cl.DisplayOrder);
            Property(cl => cl.Options);
            Property(cl => cl.ex1);
            Property(cl => cl.ex2);
        }
    }

}
