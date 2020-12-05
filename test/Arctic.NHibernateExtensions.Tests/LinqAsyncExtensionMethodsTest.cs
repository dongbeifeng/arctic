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

using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Arctic.NHibernateExtensions.Tests
{
    public class LinqAsyncExtensionMethodsTest
    {
        [Fact]
        public async Task TestAllAsync_InMemory()
        {
            var q = Enumerable.Range(1, 19).AsQueryable();

            var v1 = await q.AllAsync(x => x < 0);
            var v2 = await q.AllAsync(x => x > 0);

            Assert.False(v1);
            Assert.True(v2);
        }

        [Fact]
        public async Task TestAnyAsync1_InMemory()
        {
            var q1 = Enumerable.Range(1, 19).AsQueryable();
            var q2 = Enumerable.Range(1, 0).AsQueryable();

            var v1 = await q1.AnyAsync();
            var v2 = await q2.AnyAsync();

            Assert.True(v1);
            Assert.False(v2);
        }

        [Fact]
        public async Task TestAnyAsync2_InMemory()
        {
            var q = Enumerable.Range(1, 19).AsQueryable();

            var v1 = await q.AnyAsync(x => x < 0);
            var v2 = await q.AnyAsync(x => x > 0);

            Assert.False(v1);
            Assert.True(v2);
        }



        [Fact]
        public async Task TestToListAsync_InMemory()
        {
            var q = Enumerable.Range(1, 19).AsQueryable();

            var v1 = await q.ToListAsync();

            Assert.Equal(19, v1.Count);
        }
    }
}
