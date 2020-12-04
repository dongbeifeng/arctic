using System.Collections.Generic;

namespace Arctic.Web.Books
{
    /// <summary>
    /// 表示列表页结果
    /// </summary>
    public class BookList : OperationResult
    {
        /// <summary>
        /// 当前分页的数据
        /// </summary>
        public IEnumerable<BookListItem>? Data { get; init; }

        /// <summary>
        /// 总共有多少个数据
        /// </summary>
        public int Total { get; init; }
    }


}
