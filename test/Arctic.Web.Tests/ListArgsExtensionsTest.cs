using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Arctic.Web.Tests
{
    public class ListArgsExtensionsTest
    {
        class Foo : IListArgs<string>
        {
            [DbLike]
            public string LikeString { get; set; }

            public string NormalString { get; set; }

            public OrderedDictionary Sort { get; set; }

            public int Current { get; set; }

            public int PageSize { get; set; }

            public IQueryable<string> Filter(IQueryable<string> q)
            {
                if (NormalString != null)
                {
                    q = q.Where(x => x == NormalString);
                }
                return q;
            }
        }

        [Fact]
        public void TestNormalizePageInfo()
        {
            Foo foo = new Foo();
            foo.Current = -1;
            foo.PageSize = 0;

            ListArgsExtensions.NormalizePageInfo(foo);

            Assert.Equal(1, foo.Current);
            Assert.Equal(10, foo.PageSize);
        }

        [Fact]
        public void TestNormalizeSortInfo()
        {
            Foo foo = new Foo();
            foo.Sort = new OrderedDictionary();
            foo.Sort["a"] = "descending";
            foo.Sort["b"] = "desc";
            foo.Sort["c"] = "";

            ListArgsExtensions.NormalizeSortInfo(foo);

            Assert.Equal("DESC", foo.Sort["a"]);
            Assert.Equal("DESC", foo.Sort["b"]);
            Assert.Equal("ASC", foo.Sort["c"]);
        }


        [Fact]
        public void NormalizeStringProperties_¿Õ°××Ö·û´®»á×ª»»Îªnull()
        {
            Foo foo = new Foo();
            foo.NormalString = "  ";
            foo.LikeString = "  ";

            ListArgsExtensions.NormalizeStringProperties(foo);

            Assert.Null(foo.NormalString);
            Assert.Null(foo.LikeString);
        }

        [Fact]
        public void NormalizeStringProperties_Ê×Î²¿Õ°×»á±»ÒÆ³ý()
        {
            Foo foo = new Foo();
            foo.NormalString = " a B    ";
            foo.LikeString = " B*? ";

            ListArgsExtensions.NormalizeStringProperties(foo);

            Assert.Equal("a B", foo.NormalString);
            Assert.Equal("B%_", foo.LikeString);
        }

        [Fact]
        public async Task TestListAsync()
        {
            var list = new List<string>
            {
                "A", "A", "A", "A",
                "B", "B", "B", "B", "B",
                "C", "C", "C", "C", "C", "C",
            }.AsQueryable();

            Foo foo = new Foo();
            foo.NormalString = " B    ";
            foo.PageSize = 3;
            foo.Current = 2;

            var (items, total) = await list.FilterByAsync(foo);

            Assert.Equal(5, total);
            Assert.True(items.SequenceEqual(new[] { "B", "B" }));
        }
    }

}
