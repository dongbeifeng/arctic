using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Arctic.Web
{
    /// <summary>
    /// 表示操作结果
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// 指示操作是否成功
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// 操作结果的消息说明
        /// </summary>
        public string Message { get; init; }
    }


}
