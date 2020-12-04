using Arctic.Books;
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
    public class BookListArgs : IListArgs<Book>
    {
        /// <summary>
        /// 标题，支持模糊查找
        /// </summary>
        [ListFilter(ListFilterOperator.Like)]
        public string? Title { get; set; }

        /// <summary>
        /// 出版日期
        /// </summary>
        [ListFilter("PublicationDate", ListFilterOperator.GTE )]
        public DateTime? PublicationDateFrom { get; set; }


        /// <summary>
        /// 出版日期
        /// </summary>
        [ListFilter("PublicationDate", ListFilterOperator.LT)]
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
