using Arctic.Auditing;
using NHibernate;
using NHibernate.Type;
using System;
using System.Security.Principal;
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
        readonly IPrincipal? _principal;
        public AuditInterceptor(IPrincipal? principal)
        {
            _principal = principal;
        }

        private string GetCurrentUserName()
        {
            return (_principal?.Identity?.Name) ?? "-";
        }
        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            bool modified = false;
            if (entity is IHasMtime)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if (nameof(IHasMtime.mtime).Equals(propertyNames[i]))
                    {
                        currentState[i] = DateTime.Now;
                        modified = true;
                    }
                }
            }

            if (entity is IHasMuser)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if (nameof(IHasMuser.muser).Equals(propertyNames[i]))
                    {
                        currentState[i] = GetCurrentUserName();
                        modified = true;
                    }
                }
            }

            return modified;
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            bool modified = false;

            if (entity is IHasCtime)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if (nameof(IHasCtime.ctime).Equals(propertyNames[i]))
                    {
                        state[i] = DateTime.Now;
                        modified = true;
                    }
                }
            }

            if (entity is IHasCuser)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if (nameof(IHasCuser.cuser).Equals(propertyNames[i]) && state[i] == null)
                    {
                        state[i] = GetCurrentUserName();
                        modified = true;
                    }
                }
            }

            if (entity is IHasMtime)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if (nameof(IHasMtime.mtime).Equals(propertyNames[i]))
                    {
                        state[i] = DateTime.Now;
                        modified = true;
                    }
                }
            }

            if (entity is IHasMuser)
            {
                for (int i = 0; i < propertyNames.Length; i++)
                {
                    if (nameof(IHasMuser.muser).Equals(propertyNames[i]))
                    {
                        state[i] = GetCurrentUserName();
                        modified = true;
                    }
                }
            }

            return modified;
        }
    }
}
