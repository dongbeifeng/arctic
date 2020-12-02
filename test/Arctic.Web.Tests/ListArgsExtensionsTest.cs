using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Arctic.Web.ListFilterOperator;

namespace Arctic.Web.Tests
{
    public class ListArgsExtensionsTest
    {
        class Foo
        {
            public string Title { get; set; }

            public string Author { get; set; }
        }

        class FooListArgs : IListArgs<Foo>
        {
            [ListFilter(Operator = Like)]
            public string Title { get; set; }

            [ListFilter]
            public string Author { get; set; }

            public OrderedDictionary Sort { get; set; }

            public int Current { get; set; }

            public int PageSize { get; set; }

        }


        [Fact]
        public async Task TestListAsync()
        {
            var list = new List<Foo>
            {
                new Foo{ Title = "the quick brown fox jumps over a lazy dog", Author = "Fox" },
                new Foo{ Title = "the quick brown fox jumps over a lazy dog", Author = "Fox" },
                new Foo{ Title = "the quick brown dog jumps over a lazy fox", Author = "Dog" },
            }.AsQueryable();

            FooListArgs args = new FooListArgs
            {
                Author = " Fox    ",
                Title = "fox jumps*",
                PageSize = 0,
                Current = -9
            };
            var (items, current, size, total) = await list.ToPagedListAsync(args);

            Assert.Equal(2, total);
            Assert.Equal(2, items.Count);
            Assert.Equal(1, current);
            Assert.Equal(10, size);
            Assert.Equal("Fox", items[0].Author);
        }
    }

}
