using System.ComponentModel.DataAnnotations;

namespace Arctic.Web.Books
{
    /// <summary>
    /// 更新图书操作的参数
    /// </summary>
    public class UpdateBookArgs
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        public string? Title { get; init; }

        /// <summary>
        /// 作者
        /// </summary>
        [Required]
        public string? Author { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [Required]
        public virtual decimal Price { get; init; }
    }


}
