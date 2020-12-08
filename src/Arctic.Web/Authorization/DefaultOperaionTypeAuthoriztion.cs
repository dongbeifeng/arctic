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

using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Arctic.Web
{
    /// <summary>
    /// <see cref="IOperaionTypeAuthoriztion"/> 接口的默认实现
    /// </summary>
    public class DefaultOperaionTypeAuthoriztion : IOperaionTypeAuthoriztion
    {
        readonly ISessionFactory _sessionFactory;

        List<(string roleName, string opType)>? _data;

        /// <summary>
        /// 初始化新实例。
        /// </summary>
        /// <param name="sessionFactory"></param>
        public DefaultOperaionTypeAuthoriztion(ISessionFactory sessionFactory)
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
