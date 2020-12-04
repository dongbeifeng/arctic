using System;

namespace Arctic.Auditing
{
    // TODO 重命名
    /// <summary>
    /// 表示数据具有修改时间字段。
    /// </summary>
    public interface IHasMtime
    {
        /// <summary>
        /// 获取或设置数据的修改时间。
        /// </summary>
        DateTime mtime { get; set; }
    }

}
