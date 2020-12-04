using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Arctic.NHibernateExtensions;
using System.Text;
using System.Collections;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Arctic.Web.Tests")]

namespace Arctic.Web
{
    /// <summary>
    /// 为 <see cref="IListArgs{T}"/> 提供动态查询方法。
    /// </summary>
    public static class ListArgsExtensions
    {
        /// <summary>
        /// 使用列表参数进行查询，返回分页的列表。
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="q">目标类型上的查询对象</param>
        /// <param name="listArgs">列表参数</param>
        /// <returns></returns>
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> q, IListArgs<T> listArgs)
        {
            if (listArgs == null)
            {
                throw new ArgumentNullException(nameof(listArgs));
            }

            Type argsType = listArgs.GetType();

            //MethodInfo methodInfo = argsType.GetMethod("Filter", new[] { typeof(IQueryable<T>) });
            //if (methodInfo?.ReturnType == typeof(IQueryable<T>))
            //{

            //}
            // TODO 实现功能：如果 listArgs 上有 Filter<T>(IListArgs<T>) 方法，则首先调用 Filter 方法
            var props = argsType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttribute<ListFilterAttribute>();
                if (attr == null)
                {
                    continue;
                }

                object? val = GetPropertyValue(prop, listArgs);

                if (val != null)
                {

                    string targetProperty;
                    if (attr.TargetProperty != null)
                    {
                        targetProperty = attr.TargetProperty;
                    }
                    else
                    {
                        targetProperty = prop.Name;
                    }

                    switch (attr.Operator)
                    {
                        case ListFilterOperator.E:
                            q = q.Where($"{targetProperty} == @0", val);
                            break;
                        case ListFilterOperator.Like:
                            q = q.Where(CreateLikeExpression<T>(targetProperty, (string)val));
                            break;
                        case ListFilterOperator.GT:
                            q = q.Where($"{targetProperty} > @0", val);
                            break;
                        case ListFilterOperator.GTE:
                            q = q.Where($"{targetProperty} >= @0", val);
                            break;
                        case ListFilterOperator.LT:
                            q = q.Where($"{targetProperty} < @0", val);
                            break;
                        case ListFilterOperator.LTE:
                            q = q.Where($"{targetProperty} <= @0", val);
                            break;
                        default:
                            break;
                    }
                }
            }

            string? orderBy = GetOrderByClause(listArgs.Sort);
            if (string.IsNullOrWhiteSpace(orderBy) == false)
            {
                q = q.OrderBy(orderBy);
            }

            int current = listArgs.Current > 0 ? listArgs.Current.Value : 1;
            int pageSize = listArgs.PageSize > 0 ? listArgs.PageSize.Value : 10;
            var totalItemCount = q.Count();
            if (totalItemCount == 0)
            {
                return new PagedList<T>(new List<T>(), current, pageSize, totalItemCount);
            }

            int start = (current - 1) * pageSize;
            var list = await q.Skip(start)
                .Take(pageSize)
                .ToListAsync()
                .ConfigureAwait(false);
            return new PagedList<T>(list, current, pageSize, totalItemCount);
        }


        static object? GetPropertyValue<T>(PropertyInfo prop, IListArgs<T> listArgs)
        {
            var attr = prop.GetCustomAttribute<ListFilterAttribute>();
            if (attr == null)
            {
                return null;
            }

            object? val = prop.GetValue(listArgs);

            // 处理字符串类型的查询条件
            if (prop.PropertyType == typeof(string))
            {
                string? str = (string?)val;

                // 忽略空白字符串
                if (string.IsNullOrWhiteSpace(str))
                {
                    return null;
                }

                str = str.Trim();

                // like 需专门处理
                if (attr.Operator == ListFilterOperator.Like)
                {
                    str = str.Replace('*', '%').Replace('?', '_');
                }

                val = str;
            }

            return val;
        }

        static string? GetOrderByClause(OrderedDictionary? sort)
        {
            if (sort == null || sort.Count == 0)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            foreach (DictionaryEntry entry in sort)
            {
                string sortOrder = entry.Value?.ToString()?.ToLower() switch
                {
                    "desc" 
                    or "descend" 
                    or "descending" => "DESC",
                    _ => "ASC"
                };

                sb.Append($"{entry.Key} {sortOrder}, ");
            }
            sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }

        static Expression<Func<T, bool>> CreateLikeExpression<T>(string targetPropertyName, string likePattern)
        {
            // x => SqlMethods.Like(x.StringField, "a%")
            MethodInfo? mi = typeof(SqlMethods).GetMethod("Like", new Type[] { typeof(string), typeof(string) });
            if (mi == null)
            {
                throw new InvalidOperationException();
            }
            ParameterExpression x = Expression.Parameter(typeof(T), "x");  // x
            Expression stringField = Expression.Property(x, targetPropertyName);   // x.StringField
            Expression pattern = Expression.Constant(likePattern, typeof(string));  // 'a%'
            Expression like = Expression.Call(null, mi, stringField, pattern); //  SqlMethods.Like(x.StringField, "a%")

            return Expression.Lambda<Func<T, bool>>(like, new ParameterExpression[] { x });
        }


        /// <summary>
        /// 代替 <see cref="NHibernate.Linq.SqlMethods.Like(string, string)"/>，以提供内存里的简单模糊查询。
        /// </summary>
        private static class SqlMethods
        {
            public static bool Like(string input, string pattern)
            {
                pattern = Regex.Escape(pattern).Replace("%", ".*").Replace("_", ".");
                return Regex.IsMatch(input, pattern);
            }
        }
    }
}



