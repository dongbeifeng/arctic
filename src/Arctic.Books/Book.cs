using Arctic.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Arctic.Books
{
    /// <summary>
    /// 表示图书的实体。所有公共属性和方法必须为 virtual。
    /// </summary>
    public class Book : IHasCuser, IHasCtime, IHasMuser, IHasMtime
    {
        /// <summary>
        /// 主键。使用 int 类型，使用 XId 的形式命名。
        /// </summary>
        public virtual int BookId { get; set; }

        /// <summary>
        /// <see cref="RequiredAttribute"/> 指示属性不可为 null，<see cref="MaxLengthAttribute"/> 指示字段的最大长度。
        /// </summary>
        [Required]
        [MaxLength(50)]
        public virtual string Title { get; set; }

        [Required]
        [MaxLength(50)]
        public virtual string Author { get; set; }

        /// <summary>
        /// 值类型天然不可为 null。
        /// </summary>
        public virtual DateTime PublicationDate { get; set; }

        public virtual decimal Price { get; set; }

        #region auditing properties

        #endregion
        public virtual string cuser { get; set; }
        public virtual string muser { get; set; }
        public virtual DateTime mtime { get; set; }
        public virtual DateTime ctime { get; set; }
    }
}
