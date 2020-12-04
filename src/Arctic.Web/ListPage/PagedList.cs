using System.Collections.Generic;

namespace Arctic.Web
{
    /// <summary>
    /// 表示分页的列表。
    /// CurrentPage 基于 1。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public record PagedList<T>(List<T> List, int CurrentPage, int PageSize, int Total);

}
