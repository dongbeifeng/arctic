using System;

namespace Arctic.Web
{
    /// <summary>
    /// 用于修饰 <see cref="IListArgs{T}"/> 的字符串类型属性，表示这个属性使用数据库的 LIKE 操作符进行查询。
    /// 此标记的作用是告知 <see cref="ListArgsExtensions.NormalizeStringProperties{T}(IListArgs{T})"/> 方法
    /// 将字符串中的 <c>*</c> 替换成 <c>%</c>，将 <c>?</c> 替换成 <c>_</c>。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DbLikeAttribute : Attribute
    {
    }

}
