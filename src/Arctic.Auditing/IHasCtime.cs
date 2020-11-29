using System;

namespace Arctic.Auditing
{
    // TODO 重命名
    /// <summary>
    /// 表示数据具有创建时间字段。
    /// </summary>
    public interface IHasCtime
    {
        /// <summary>
        /// 获取或设置数据的创建时间。
        /// </summary>
        DateTime ctime { get; set; }
    }

}
