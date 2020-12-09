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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using static Arctic.Web.ListFilterOperator;

namespace Arctic.Web.Tests
{
    public class ListArgsExtensionsTest
    {
        class Foo
        {
            public string? Title { get; set; }

            public string? Author { get; set; }
        }

        class NormalFooListArgs : IListArgs<Foo>
        {
            [ListFilter(ListFilterOperator.Like)]
            public string? Title { get; set; }

            [ListFilter]
            public string? Author { get; set; }


            public OrderedDictionary? Sort { get; set; }

            public int? Current { get; set; }

            public int? PageSize { get; set; }

        }

        class LinqFooListArgs : IListArgs<Foo>
        {
            [ListFilter(ListFilterOperator.Linq)]
            public string? Title { get; set; }

            internal Expression<Func<Foo, bool>>? TitleExpr
            {
                get
                {
                    if (Title == null)
                    {
                        return null;
                    }
                    return x => x.Title != null && x.Title.Contains(Title);
                }
            }

            public OrderedDictionary? Sort { get; set; }

            public int? Current { get; set; }

            public int? PageSize { get; set; }

        }

        class InFooListArgs : IListArgs<Foo>
        {
            [ListFilter(ListFilterOperator.IN)]
            public string[]? Author { get; set; }

            public OrderedDictionary? Sort { get; set; }

            public int? Current { get; set; }

            public int? PageSize { get; set; }

        }


        [Fact]
        public async Task TestToPagedListNormalAsync()
        {
            var list = new List<Foo>
            {
                new Foo{ Title = "the quick brown fox jumps over a lazy dog", Author = "Fox" },
                new Foo{ Title = "the quick brown fox jumps over a lazy dog", Author = "Fox" },
                new Foo{ Title = "the quick brown dog jumps over a lazy fox", Author = "Dog" },
            }.AsQueryable();

            NormalFooListArgs args = new NormalFooListArgs
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

        [Fact]
        public async Task TestToPagedListLinqAsync()
        {
            var list = new List<Foo>
            {
                new Foo{ Title = "the quick brown fox jumps over a lazy dog", Author = "Fox" },
                new Foo{ Title = "the quick brown fox jumps over a lazy dog", Author = "Fox" },
                new Foo{ Title = "the quick brown dog jumps over a lazy fox", Author = "Dog" },
            }.AsQueryable();

            LinqFooListArgs args = new LinqFooListArgs
            {
                Title = "fox jumps",
            };
            var (items, current, size, total) = await list.ToPagedListAsync(args);

            Assert.Equal(2, total);
            Assert.Equal(2, items.Count);
            Assert.Equal(1, current);
            Assert.Equal(10, size);
            Assert.Equal("Fox", items[0].Author);
        }

        [Fact]
        public async Task TestToPagedListInAsync()
        {
            var list = new List<Foo>
            {
                new Foo{ Title = "the quick brown fox jumps over a lazy dog", Author = "Fox" },
                new Foo{ Title = "the quick brown fox jumps over a lazy dog", Author = "Fox" },
                new Foo{ Title = "the quick brown dog jumps over a lazy fox", Author = "Dog" },
            }.AsQueryable();

            InFooListArgs args = new InFooListArgs
            {
                Author = new[] { "Fox" },
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
