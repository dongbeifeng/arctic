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
            public string? cuser { get; set; }
            public string? muser { get; set; }
            public string? Bar { get; set; }
        }



        [Fact]
        public void TestOnFlush()
        {
            AuditInterceptor interceptor = new AuditInterceptor(new GenericPrincipal(new GenericIdentity("wangjianjun"), null));
            Foo foo = new Foo();
            object?[] currentState = new object?[] { foo.ctime, foo.mtime, foo.cuser, foo.muser, foo.Bar! };
            object?[] previousState = new object?[] { foo.ctime, foo.mtime, foo.cuser, foo.muser, foo.Bar! };
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
            object?[] state = new object?[] { foo.ctime, foo.mtime, foo.cuser, foo.muser, foo.Bar! };
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
