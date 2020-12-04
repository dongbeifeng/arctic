using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Arctic.Web
{
    /// <summary>
    /// <see cref="IOperaionTypePermissions"/> 接口的默认实现
    /// </summary>
    public class DefaultOperaionTypePermissions : IOperaionTypePermissions
    {
        readonly ISessionFactory _sessionFactory;

        List<(string roleName, string opType)>? _data;

        /// <summary>
        /// 初始化新实例。
        /// </summary>
        /// <param name="sessionFactory"></param>
        public DefaultOperaionTypePermissions(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        /// <summary>
        /// 指定操作类型，获取允许执行此操作的角色。
        /// </summary>
        /// <param name="opType"></param>
        /// <returns></returns>
        public List<string> GetAllowedRoles(string opType)
        {
            LoadData();
            return _data!.Where(x => x.opType == opType)
                .Select(x => x.roleName)
                .Union(new[] { "admin" })
                .Distinct()
                .ToList();
        }


        private void LoadData()
        {
            lock (this)
            {
                if (_data == null)
                {
                    using var session = _sessionFactory.OpenSession();
                    using ITransaction tx = session.BeginTransaction();
                    // TODO 未完成
                    //_data = session.Query<Role>()
                    //    .ToList()
                    //    .SelectMany(role => role.AllowedOpTypes.Select(opType => (role.RoleName, opType)))
                    //    .ToList();
                    _data = new List<(string roleName, string opType)>();

                    tx.Commit();
                }
            }
        }
    }

}
