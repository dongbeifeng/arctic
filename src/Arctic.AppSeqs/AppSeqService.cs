// Copyright 2020-2021 王建军
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
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Arctic.AppSeqs
{
    public class AppSeqService : IAppSeqService
    {
        ISessionFactory _sessionFactory;

        public AppSeqService(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        /// <summary>
        /// 获取下一个序号值，如果具有指定名称的序列不存在，则创建它。
        /// 序列值从 1 开始。此方法不参与环境事务，即使事务回滚，也不会重用序列值。
        /// </summary>
        /// <param name="seqName"></param>
        /// <returns></returns>
        public async Task<int> GetNextAsync(string seqName)
        {
            int next = 0;

            // 不参与环境事务，以免事务回滚导致序列值重用
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            {
                using (ISession session = _sessionFactory.OpenSession())
                {
                    using (ITransaction tx = session.BeginTransaction())
                    {
                        AppSeq seqObject = await session
                            .GetAsync<AppSeq>(seqName, LockMode.Upgrade)
                            .ConfigureAwait(false);
                        if (seqObject == null)
                        {
                            seqObject = new AppSeq(seqName);
                            next = seqObject.GetNextVal();
                            await session
                                .SaveAsync(seqObject)
                                .ConfigureAwait(false);
                        }
                        else
                        {
                            next = seqObject.GetNextVal();
                            await session
                                .UpdateAsync(seqObject)
                                .ConfigureAwait(false);
                        }

                        tx.Commit();
                    }
                }

                scope.Complete();
            }

            return next;

        }
    }
}
