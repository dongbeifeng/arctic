using System;
using System.Collections.Generic;

namespace Arctic.Web
{
    /// <summary>
    /// 提供为操作类型获取授权的方法。
    /// </summary>
    public interface IOperaionTypePermissions
    {
        /// <summary>
        /// 指定操作类型，获取允许执行此操作的角色。
        /// </summary>
        /// <param name="operationType"></param>
        /// <returns></returns>
        List<string> GetAllowedRoles(string operationType);

    }
}
