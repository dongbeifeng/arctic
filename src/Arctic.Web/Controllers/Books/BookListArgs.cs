// Copyright 2020 王建军
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

using Arctic.Books;
using Arctic.NHibernateExtensions;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace Arctic.Web.Books
{
    /// <summary>
    /// 列表查询参数，不需要实现特定接口或从特定基类派生。
    /// </summary>
    public class BookListArgs
    {
        /// <summary>
        /// 使用 SearchArg 表示属性是查询参数。SearchMode.Like 表示支持模糊查找，
        /// 使用 ? 表示一个字符，使用 * 表示任意个字符。
        /// </summary>
        [SearchArg(SearchMode.Like)]
        public string? Title { get; set; }

        /// <summary>
        /// 当源属性名称和参数不同时，SourceProperty 指示在数据源的哪个属性上查找。
        /// </summary>
        [SourceProperty(nameof(Book.PublicationDate))]
        [SearchArg(SearchMode.GreaterOrEqual)]
        public DateTime? PublicationDateFrom { get; set; }

        /// <summary>
        /// 当 SearchMode 为 Expression 时，则使用名为 PublicationDateToExpr 的属性来确定查询条件。
        /// </summary>
        [SearchArg(SearchMode.Expression)]
        public DateTime? PublicationDateTo { get; set; }

        /// <summary>
        /// 为 PublicationDateTo 属性提供查询表达式，应使用 internal 修饰符，
        /// 否则 swagger 提供的接口文档会出现 System.Reflection 命名空间中的类型。
        /// </summary>
        internal Expression<Func<Book, bool>>? PublicationDateToExpr
        {
            get
            {
                if (PublicationDateTo == null)
                {
                    return null;
                }

                return (x => x.PublicationDate < PublicationDateTo.Value);
            }
        }

        /// <summary>
        /// 名为 Filter 的公共实例方法会先被调用。参数和返回值应为 IQueryable<Book>。
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public IQueryable<Book> Filter(IQueryable<Book> q)
        {
            return q.Where(x => x.BookId < 50);
        }

        /// <summary>
        /// Sort 属性表示排序条件，字典的键表示排序字段，值表示排序顺序。
        /// 例如 { "Title": "asc", PublicationDate: "desc" } 
        /// 表示首先按 Title 属性升序排序，再按 PublicationDate 降序排序。
        /// </summary>
        public OrderedDictionary? Sort { get; set; }

        /// <summary>
        /// Current 属性表示基于 1 的当前页面。
        /// </summary>
        public int? Current { get; set; }

        /// <summary>
        /// PageSize 属性表示每个分页的大小。
        /// </summary>
        public int? PageSize { get; set; }

    }
}
