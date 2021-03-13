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
using NHibernate.Event;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Arctic.NHibernateExtensions
{
    /// <summary>
    /// 确保增删改操作在事务中进行。
    /// </summary>
    public class CheckTransactionListener : IPreInsertEventListener, IPreDeleteEventListener, IPreUpdateEventListener, IPreLoadEventListener
    {
        public bool OnPreDelete(PreDeleteEvent @event)
        {
            CheckTransaction(@event.Session);
            return false;
        }

        public Task<bool> OnPreDeleteAsync(PreDeleteEvent @event, CancellationToken cancellationToken)
        {
            OnPreDelete(@event);
            return Task.FromResult(false);
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            CheckTransaction(@event.Session);
            return false;
        }

        public Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
        {
            OnPreInsert(@event);
            return Task.FromResult(false);
        }

        public void OnPreLoad(PreLoadEvent @event)
        {
            CheckTransaction(@event.Session);
        }

        public Task OnPreLoadAsync(PreLoadEvent @event, CancellationToken cancellationToken)
        {
            OnPreLoad(@event);
            return Task.CompletedTask;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            CheckTransaction(@event.Session);
            return false;
        }

        public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
        {
            OnPreUpdate(@event);
            return Task.FromResult(false);
        }

        private void CheckTransaction(ISession session)
        {
            var tx = session.GetCurrentTransaction();
            if (tx == null || tx.IsActive == false)
            {
                throw new Exception("未打开事务。");
            }
        }
    }

}
