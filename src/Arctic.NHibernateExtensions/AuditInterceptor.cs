using Arctic.Auditing;
using NHibernate;
using NHibernate.Type;
using System;
using System.Threading;

namespace Arctic.NHibernateExtensions
{
    /// <summary>
    /// 用于为 <see cref="IHasCtime.ctime"/>, <see cref="IHasMtime.mtime"/>，
    /// <see cref="IHasCuser.cuser"/> 和 <see cref="IHasMuser.muser"/> 属性
    /// 自动赋值的 nhibernate 拦截器。
    /// </summary>
    public class AuditInterceptor : EmptyInterceptor
    {
        private static string getCurrentUserName()
        {
            return (Thread.CurrentPrincipal?.Identity?.Name) ?? "-";
        }
        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            if (entity is IHasMtime hasMtime)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if (nameof(hasMtime.mtime).Equals(propertyNames[i]))
                    {
                        currentState[i] = DateTime.Now;
                        return true;
                    }
                }
            }

            if (entity is IHasMuser hasMuser)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if (nameof(hasMuser.muser).Equals(propertyNames[i]))
                    {
                        currentState[i] = getCurrentUserName();
                        return true;
                    }
                }
            }

            return false;
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            bool modified = false;

            if (entity is IHasCtime hasCtime)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if (nameof(hasCtime.ctime).Equals(propertyNames[i]))
                    {
                        state[i] = DateTime.Now;
                        modified = true;
                    }
                }
            }

            if (entity is IHasCuser hasCuser)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if (nameof(hasCuser.cuser).Equals(propertyNames[i]) && state[i] == null)
                    {
                        state[i] = getCurrentUserName();
                        modified = true;
                    }
                }
            }

            if (entity is IHasMtime hasMtime)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if (nameof(hasMtime.mtime).Equals(propertyNames[i]))
                    {
                        state[i] = DateTime.Now;
                        modified = true;
                    }
                }
            }

            if (entity is IHasMuser hasMuser)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if (nameof(hasMuser.muser).Equals(propertyNames[i]))
                    {
                        state[i] = getCurrentUserName();
                        modified = true;
                    }
                }
            }


            return modified;
        }
    }

}
