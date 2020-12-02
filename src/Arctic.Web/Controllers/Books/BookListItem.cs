using System.Collections.Generic;

namespace Arctic.Web.Books
{
    /// <summary>
    /// 图书列表页的数据项。
    /// </summary>
    public class BookListItem
    {
        /// <summary>
        /// 图书 Id。
        /// </summary>
        public int BookId { get; init; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; init; }

        /// <summary>
        /// 价格
        /// </summary>
        public virtual decimal Price { get; init; }

    }



    /// <summary>
    /// 表示列表页结果
    /// </summary>
    public class BookListResult : OperationResult
    {
        /// <summary>
        /// 当前分页的数据
        /// </summary>
        public IEnumerable<BookListItem> Data { get; init; }

        /// <summary>
        /// 总共有多少个数据
        /// </summary>
        public int Total { get; init; }
    }


}
