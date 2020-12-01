using Arctic.Auditing;
using NHibernate.Type;
using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Arctic.NHibernateExtensions.Tests
{
    public class AuditInterceptorTest
    {
        private class Foo : IHasCtime, IHasMtime, IHasCuser, IHasMuser
        {
            public System.DateTime ctime { get; set; } = DateTime.MinValue;
            public System.DateTime mtime { get; set; } = DateTime.MinValue;
            public string cuser { get; set; }
            public string muser { get; set; }
            public string Bar { get; set; }
        }

        private class SetUser : IDisposable
        {
            IPrincipal principal = null;
            public SetUser()
            {
                principal = Thread.CurrentPrincipal;
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("wangjianjun"), null);

            }
            public void Dispose()
            {
                Thread.CurrentPrincipal = principal;
            }
        }

        [Fact]
        public async Task TestOnFlush()
        {
            AuditInterceptor interceptor = new AuditInterceptor();
            Foo foo = new Foo();
            object[] currentState = new object[] { foo.ctime, foo.mtime, foo.cuser, foo.muser, foo.Bar };
            object[] previousState = new object[] { foo.ctime, foo.mtime, foo.cuser, foo.muser, foo.Bar };
            string[] propertiesNames = new[] { "ctime", "mtime", "cuser", "muser", "Bar" };
            IType[] types = new IType[] { TypeFactory.GetDateTimeType(4), TypeFactory.GetDateTimeType(4), TypeFactory.GetStringType(10), TypeFactory.GetStringType(10), TypeFactory.GetStringType(10) };
            using (new SetUser())
            {
                var b = interceptor.OnFlushDirty(
                    foo,
                    "FOO",
                    currentState,
                    previousState,
                    propertiesNames,
                    types
                    );

                Assert.True(b);
                Assert.NotEqual(DateTime.MinValue, currentState[1]);
                Assert.Equal("wangjianjun", currentState[3]);
                Assert.Null(currentState[4]);
            }

        }

        [Fact]
        public async Task TestOnSave()
        {
            AuditInterceptor interceptor = new AuditInterceptor();
            Foo foo = new Foo();
            object[] state = new object[] { foo.ctime, foo.mtime, foo.cuser, foo.muser, foo.Bar };
            string[] propertiesNames = new[] { "ctime", "mtime", "cuser", "muser", "Bar" };
            IType[] types = new IType[] { TypeFactory.GetDateTimeType(4), TypeFactory.GetDateTimeType(4), TypeFactory.GetStringType(10), TypeFactory.GetStringType(10), TypeFactory.GetStringType(10) };
            using (new SetUser())
            {
                var b = interceptor.OnSave(
                    foo,
                    "FOO",
                    state,
                    propertiesNames,
                    types
                    );

                Assert.True(b);
                Assert.NotEqual(DateTime.MinValue, state[0]);
                Assert.NotEqual(DateTime.MinValue, state[1]);
                Assert.Equal("wangjianjun", state[2]);
                Assert.Equal("wangjianjun", state[3]);
                Assert.Null(state[4]);
            }

        }

    }

}
