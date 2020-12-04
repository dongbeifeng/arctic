namespace Arctic.Auditing
{
    // TODO 重命名
    /// <summary>
    /// 表示数据具有创建人字段。
    /// </summary>
    public interface IHasCuser
    {
        /// <summary>
        /// 获取或设置数据的创建人。
        /// </summary>
        string cuser { get; set; }
    }

}
