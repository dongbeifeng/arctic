using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Arctic.NHibernateExtensions;
using System.Text;
using System.Collections;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Arctic.Web.Tests")]

namespace Arctic.Web
{
    public static class ListArgsExtensions
    {
        public static void Normalize<T>(this IListArgs<T> listArgs)
        {
            if (listArgs == null)
            {
                return;
            }

            NormalizePageInfo(listArgs);
            NormalizeSortInfo(listArgs);
            NormalizeStringProperties(listArgs);
        }

        internal static void NormalizePageInfo<T>(IListArgs<T> listArgs)
        {
            if (listArgs.Current < 1)
            {
                listArgs.Current = 1;
            }
            if (listArgs.PageSize <= 0)
            {
                listArgs.PageSize = 10;
            }
        }

        internal static void NormalizeSortInfo<T>(IListArgs<T> listArgs)
        {
            if (listArgs.Sort == null)
            {
                return;
            }


            foreach (var key in listArgs.Sort.Keys.Cast<string>().ToArray())
            {
                switch (listArgs.Sort[key]?.ToString()?.ToLower())
                {
                    case "desc":
                    case "descend":
                    case "descending":
                        listArgs.Sort[key] = "DESC";
                        break;
                    default:
                        listArgs.Sort[key] = "ASC";
                        break;
                }
            }
        }

        internal static void NormalizeStringProperties<T>(IListArgs<T> listArgs)
        {
            PropertyInfo[] propertyInfos = listArgs.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    string val = (string)propertyInfo.GetValue(listArgs);
                    if (string.IsNullOrWhiteSpace(val))
                    {
                        propertyInfo.SetValue(listArgs, null);
                    }
                    else if (propertyInfo.IsDefined(typeof(DbLikeAttribute)))
                    {
                        propertyInfo.SetValue(listArgs, val.Trim().Replace('*', '%').Replace('?', '_'));
                    }
                    else
                    {
                        propertyInfo.SetValue(listArgs, val.Trim());
                    }
                }
            }
        }


        public static async Task<(List<T> list, int totalItemCount)> FilterByAsync<T>(this IQueryable<T> q, IListArgs<T> listArgs)
        {
            if (listArgs == null)
            {
                throw new ArgumentNullException(nameof(listArgs));
            }

            listArgs.Normalize();

            q = listArgs.Filter(q);

            if (listArgs.Sort != null && listArgs.Sort.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (DictionaryEntry entry in listArgs.Sort)
                {
                    sb.Append($"{entry.Key} {entry.Value}, ");
                }
                sb.Remove(sb.Length - 2, 2);
                string orderBy = sb.ToString();
                q = q.OrderBy(orderBy);
            }

            var totalItemCount = q.Count();

            if (totalItemCount == 0)
            {
                return (new List<T>(), 0);
            }

            int start = (listArgs.Current - 1) * listArgs.PageSize;
            var list = await q.Skip(start)
                .Take(listArgs.PageSize)
                .ToListAsync()
                .ConfigureAwait(false);
            return (list, totalItemCount);
        }
    }

}
