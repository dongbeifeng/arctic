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
