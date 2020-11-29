using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Arctic.Web
{
    public class DefaultOperaionTypePermissions : IOperaionTypePermissions
    {
        readonly ISessionFactory _sessionFactory;

        List<(string roleName, string opType)> _data;

        public DefaultOperaionTypePermissions(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public List<string> GetAllowedRoles(string opType)
        {
            LoadData();
            return _data.Where(x => x.opType == opType)
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
                    using (var session = _sessionFactory.OpenSession())
                    {
                        using (ITransaction tx = session.BeginTransaction())
                        {
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
    }

}
