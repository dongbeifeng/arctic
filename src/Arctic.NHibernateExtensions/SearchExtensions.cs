// Copyright 2020 王建军
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Collections.Specialized;
using System.Text;
using System.Collections;
using Serilog;

namespace Arctic.NHibernateExtensions
{
    /// <summary>
    /// 为 <see cref="ISearchArgs{T}"/> 提供动态查询方法。
    /// </summary>
    public static class SearchExtensions
    {
        static ILogger _logger = Serilog.Log.ForContext(typeof(SearchExtensions));

        /// <summary>
        /// 使用列表参数进行筛选。
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="q">目标类型上的查询对象</param>
        /// <param name="searchArgs">查询参数</param>
        /// <returns></returns>
        public static IQueryable<T> Filter<T>(this IQueryable<T> q, object searchArgs)
        {
            if (searchArgs == null)
            {
                throw new ArgumentNullException(nameof(searchArgs));
            }

            Type argsType = searchArgs.GetType();

            // 如果 searchArgs 上有 Filter 方法，则首先调用 Filter 方法
            MethodInfo? filterMethodInfo = argsType.GetMethod("Filter", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (filterMethodInfo != null 
                && filterMethodInfo.GetParameters().Length == 1 && filterMethodInfo.GetParameters()[0].ParameterType == typeof(IQueryable<T>)
                && filterMethodInfo.ReturnType == typeof(IQueryable<T>))
            {
                q = (IQueryable<T>)filterMethodInfo.Invoke(searchArgs, new[] { q })!;
                if (q == null)
                {
                    throw new InvalidOperationException("Filter 方法不能返回 null");
                }
            }

            var props = argsType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var argProp in props)
            {
                SearchArgAttribute? searchModeAttribute = argProp.GetCustomAttribute<SearchArgAttribute>();
                if (searchModeAttribute == null)
                {
                    continue;
                }

                object? argVal = GetPropertyValue(argProp, searchArgs);

                //  查询参数属性为 null 表示忽略这个条件
                if (argVal == null)
                {
                    continue;
                }

                // 源属性
                string sourcePropertyName = argProp.Name;
                var sourcePropertyAttribute = argProp.GetCustomAttribute<SourcePropertyAttribute>();
                if (sourcePropertyAttribute != null)
                {
                    sourcePropertyName = sourcePropertyAttribute.PropertyName;
                }

                if (typeof(T).GetProperty(sourcePropertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) == null)
                {
                    throw new InvalidOperationException($"类型 {typeof(T)} 上没有名为 {sourcePropertyName} 的属性");
                }


                switch (searchModeAttribute.SeachMode)
                {
                    case SearchMode.Equal:
                            q = q.Where($"{sourcePropertyName} == @0", argVal);
                        break;
                    case SearchMode.Like:
                            q = q.Where(CreateLikeExpression<T>(sourcePropertyName, (string)argVal));
                        break;
                    case SearchMode.GreaterThan:
                            q = q.Where($"{sourcePropertyName} > @0", argVal);
                        break;
                    case SearchMode.GreaterOrEqual:
                            q = q.Where($"{sourcePropertyName} >= @0", argVal);
                        break;
                    case SearchMode.Less:
                            q = q.Where($"{sourcePropertyName} < @0", argVal);
                        break;
                    case SearchMode.LessOrEqual:
                            q = q.Where($"{sourcePropertyName} <= @0", argVal);
                        break;
                    case SearchMode.In:
                            if (argVal is Array arr && arr.Length > 0)
                            {
                                q = q.Where($"@0.Contains({sourcePropertyName})", argVal);
                            }
                        break;
                    case SearchMode.Expression:
                        string exprPropName = argProp.Name + "Expr";

                        PropertyInfo? exprProp = argsType.GetProperty(exprPropName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                        if (exprProp == null)
                        {
                            throw new InvalidOperationException($"参数类型 {typeof(T)} 上为 {sourcePropertyName} 提供表达式的 {exprPropName} 属性不存在");
                        }

                        object? e = exprProp.GetValue(searchArgs, null);
                        if (e == null)
                        {
                            continue;
                        }

                        if (e is Expression<Func<T, bool>> expr)
                        {
                            q = q.Where(expr);
                        }
                        else
                        {
                            throw new InvalidOperationException($"参数类型 {typeof(T)} 的 {exprPropName} 属性的类型不是 {typeof(Expression<Func<T, bool>>)}");
                        }
                        break;
                    default:
                            throw new NotSupportedException("不支持的查询操作");
                }
            }

            return q;
        }


        /// <summary>
        /// 对查询排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q">要排序的查询对象</param>
        /// <param name="sortInfo">排序信息。键为要排序的属性，值为排序顺序，
        /// 值为 desc、descend、descending 表示降序，否则为升序。值不区分大小写。
        /// 例如 { "A" : "desc", "B": null } 表示先按属性 A 降序排序，再按属性 B 升序排序。</param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> q, OrderedDictionary?  sortInfo)
        {
            string? orderBy = GetOrderByClause(sortInfo);
            if (string.IsNullOrWhiteSpace(orderBy) == false)
            {
                q = q.OrderBy(orderBy);
            }
            return q;
        }


        /// <summary>
        /// 对查询分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <param name="currentPage">基于 1 的页号码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> q, int currentPage, int pageSize)
        {
            if (currentPage < 1)
            {
                currentPage = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            var total = q.Count();
            if (total == 0)
            {
                return new PagedList<T>(new List<T>(), 1, pageSize, 0);
            }

            int start = (currentPage - 1) * pageSize;
            var list = await q.Skip(start)
                .Take(pageSize)
                .WrappedToListAsync()
                .ConfigureAwait(false);
            return new PagedList<T>(list, currentPage, pageSize, total);
        }

        /// <summary>
        /// 在查询上进行筛选、排序、分页。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <param name="searchArgs">搜索参数</param>
        /// <param name="sortInfo">排序信息</param>
        /// <param name="currentPage">基于 1 的页号码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        public static Task<PagedList<T>> SearchAsync<T>(this IQueryable<T> q, object searchArgs, OrderedDictionary?  sortInfo, int? currentPage, int? pageSize)
        {
            return q.Filter(searchArgs).OrderBy(sortInfo).ToPagedListAsync(currentPage ?? 1, pageSize ?? 10);
        }

        static object? GetPropertyValue(PropertyInfo prop, object searchArgs)
        {
            object? val = prop.GetValue(searchArgs);

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
                if (prop.GetCustomAttribute<SearchArgAttribute>()?.SeachMode == SearchMode.Like)
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
            // 要得到的表达式：x => SqlMethods.Like(x.StringField, "a%")
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
    }
}
