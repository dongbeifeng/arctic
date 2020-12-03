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
            public string cuser { get; set; } = default!;
            public string muser { get; set; } = default!;
            public string? Bar { get; set; }
        }



        [Fact]
        public void TestOnFlush()
        {
            AuditInterceptor interceptor = new AuditInterceptor(new GenericPrincipal(new GenericIdentity("wangjianjun"), null));
            Foo foo = new Foo();
            object[] currentState = new object[] { foo.ctime, foo.mtime, foo.cuser, foo.muser, foo.Bar! };
            object[] previousState = new object[] { foo.ctime, foo.mtime, foo.cuser, foo.muser, foo.Bar! };
            string[] propertiesNames = new[] { "ctime", "mtime", "cuser", "muser", "Bar" };
            IType[] types = new IType[] { TypeFactory.GetDateTimeType(4), TypeFactory.GetDateTimeType(4), TypeFactory.GetStringType(10), TypeFactory.GetStringType(10), TypeFactory.GetStringType(10) };
            
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

        [Fact]
        public void TestOnSave()
        {
            AuditInterceptor interceptor = new AuditInterceptor(new GenericPrincipal(new GenericIdentity("wangjianjun"), null));
            Foo foo = new Foo();
            object[] state = new object[] { foo.ctime, foo.mtime, foo.cuser, foo.muser, foo.Bar! };
            string[] propertiesNames = new[] { "ctime", "mtime", "cuser", "muser", "Bar" };
            IType[] types = new IType[] { TypeFactory.GetDateTimeType(4), TypeFactory.GetDateTimeType(4), TypeFactory.GetStringType(10), TypeFactory.GetStringType(10), TypeFactory.GetStringType(10) };

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
