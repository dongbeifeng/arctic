using Arctic.Books;
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
        [DbLike]
        public string Title { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public OrderedDictionary Sort { get; set; }

        /// <summary>
        /// 基于 1 的当前页面。
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }

        public IQueryable<Book> Filter(IQueryable<Book> q)
        {
            if (Title != null)
            {
                q = q.Where(x => x.Title == Title);
            }

            return q;
        }
    }
}
