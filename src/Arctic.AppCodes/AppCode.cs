using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Arctic.AppCodes
{
    /// <summary>
    /// 表示应用程序中的基础代码，例如 WMS 的业务类型，库存状态等。
    /// </summary>
    public class AppCode : IEquatable<AppCode>
    {
        /// <summary>
        /// 初始化新实例
        /// </summary>
        public AppCode()
        {
        }

        /// <summary>
        /// 代码类型，例如业务类型，库存状态等。
        /// </summary>
        [Required]
        [MaxLength(20)]
        public virtual string AppCodeType { get; set; } = default!;


        /// <summary>
        /// 代码值，例如独立入库、合格。
        /// </summary>
        [Required]
        [MaxLength(30)]
        public virtual string AppCodeValue { get; set; } = default!;

        [MaxLength(255)]
        public virtual string? Description { get; set; }

        /// <summary>
        /// 是否对用户可见。
        /// </summary>
        public virtual bool Visible { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(255)]
        public virtual string? Comment { get; set; }

        /// <summary>
        /// 适用范围，例如业务类型中有些适用于出库单，有些适用于入库单。
        /// </summary>
        [MaxLength(20)]
        public virtual string? Scope { get; set; }

        /// <summary>
        /// 展示次序。
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// 选项。
        /// </summary>
        [MaxLength(9999)]
        public virtual string? Options { get; set; }

        [MaxLength(9999)]
        public virtual string? ex1 { get; set; }

        [MaxLength(9999)]
        public virtual string? ex2 { get; set; }


        public override bool Equals(object? obj)
        {
            return Equals(obj as AppCode);
        }

        public virtual bool Equals(AppCode? other)
        {
            return other != null &&
                   AppCodeType == other.AppCodeType &&
                   AppCodeValue == other.AppCodeValue;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AppCodeType, AppCodeValue);
        }

        public static bool operator ==(AppCode? left, AppCode? right)
        {
            return EqualityComparer<AppCode>.Default.Equals(left, right);
        }

        public static bool operator !=(AppCode? left, AppCode? right)
        {
            return !(left == right);
        }
    }

}
