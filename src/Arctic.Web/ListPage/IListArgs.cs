using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Arctic.Web
{
    /// <summary>
    /// 定义列表页参数。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IListArgs<T>
    {
        /// <summary>
        /// 获取或设置排序信息。
        /// </summary>
        OrderedDictionary Sort { get; set; }

        /// <summary>
        /// 获取或设置基于 1 的分页索引。
        /// </summary>
        int Current { get; set; }

        /// <summary>
        /// 获取或设置每页大小。
        /// </summary>
        int PageSize { get; set; }


        /// <summary>
        /// 在查询对象上应用筛选条件。
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        IQueryable<T> Filter(IQueryable<T> q);
    }

}
