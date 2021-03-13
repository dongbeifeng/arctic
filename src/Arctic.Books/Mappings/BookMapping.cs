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

namespace Arctic.Books.Mappings
{
    internal class BookMapping : ClassMapping<Book>
    {
        public BookMapping()
        {
            Table("Books");
            DynamicUpdate(true);
            BatchSize(10);

            Id(cl => cl.BookId, id => id.Generator(Generators.Identity));

            Property(cl => cl.ctime, prop => prop.Update(false));
            Property(cl => cl.cuser, prop => prop.Update(false));
            Property(cl => cl.mtime);
            Property(cl => cl.Title);
            Property(cl => cl.Author);
            Property(cl => cl.Price);
            Property(cl => cl.PublicationDate);
        }
    }
}
