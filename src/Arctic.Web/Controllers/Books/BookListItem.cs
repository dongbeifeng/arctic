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
        public int BookId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public virtual decimal Price { get; set; }

    }
}
