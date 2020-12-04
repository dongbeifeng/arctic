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
