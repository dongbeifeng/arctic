using System;
using System.ComponentModel.DataAnnotations;

namespace Arctic.Web.Books
{
    /// <summary>
    /// 创建图书操作的参数
    /// </summary>
    public class CreateBookArgs
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        public string? Title { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [Required]
        public string? Author { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 出版日期
        /// </summary>
        [Required]
        public DateTime? PublicationDate { get; set; }
    }


}
