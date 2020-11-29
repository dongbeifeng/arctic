namespace Arctic.Auditing
{
    // TODO 重命名
    /// <summary>
    /// 表示数据具有修改人字段。
    /// </summary>
    public interface IHasMuser
    {
        /// <summary>
        /// 获取或设置数据的修改人。
        /// </summary>
        string muser { get; set; }
    }

}
