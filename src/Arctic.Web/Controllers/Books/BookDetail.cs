namespace Arctic.Web.Books
{
    /// <summary>
    /// 图书详细页的数据项。
    /// </summary>
    public class BookDetail
    {
        /// <summary>
        /// 图书 Id。
        /// </summary>
        public int BookId { get; init; }

        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; init; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public virtual decimal Price { get; init; }

    }


}
