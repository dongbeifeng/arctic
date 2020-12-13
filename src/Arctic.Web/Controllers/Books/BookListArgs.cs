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
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Arctic.Web.Books
{
    /// <summary>
    /// 图书列表查询参数
    /// </summary>
    public class BookListArgs
    {
        /// <summary>
        /// 标题，支持模糊查找
        /// </summary>
        [SearchArg(SearchMode.Like)]
        public string? Title { get; set; }

        /// <summary>
        /// 出版日期
        /// </summary>
        [SourceProperty("PublicationDate")]
        [SearchArg(SearchMode.GreaterOrEqual)]
        public DateTime? PublicationDateFrom { get; set; }


        /// <summary>
        /// 出版日期
        /// </summary>
        [SourceProperty("PublicationDate")]
        [SearchArg(SearchMode.Less)]
        public DateTime? PublicationDateTo { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public OrderedDictionary? Sort { get; set; }

        /// <summary>
        /// 基于 1 的当前页面。
        /// </summary>
        public int? Current { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int? PageSize { get; set; }

    }
}
