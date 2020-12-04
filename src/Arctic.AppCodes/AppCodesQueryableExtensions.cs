using Arctic.NHibernateExtensions;
using Autofac;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arctic.AppCodes
{
    public static class AppCodesQueryableExtensions
    {
        public static Task<List<AppCode>> GetAppCodesAsync(this IQueryable<AppCode> q, string appCodeType, bool visibleOnly = true)
        {
            if (visibleOnly)
            {
                q = q.Where(x => x.Visible == true);
            }

            return q
                .Where(x => x.AppCodeType == appCodeType)
                .OrderBy(x => x.DisplayOrder)
                .ToListAsync();
        }
    }
}
