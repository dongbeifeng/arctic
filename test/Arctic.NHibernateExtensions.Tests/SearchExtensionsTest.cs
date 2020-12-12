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

namespace Arctic.NHibernateExtensions.Web.Tests
{
    public class SearchExtensionsTest
    {
        #region nested classes

        class Student
        {
            public string Name { get; init; } = default!;

            public int No { get; init; }
        }

        class EqualArgs
        {
            [SearchArg]
            public string? Name { get; set; }

            [SearchArg(SearchMode.Equal)]
            public int? No { get; set; }
        }

        class LikeArgs
        {
            [SearchArg(SearchMode.Like)]
            public string? Name { get; set; }
        }

        class GreaterThanArgs
        {
            [SearchArg(SearchMode.GreaterThan)]
            public int? No { get; set; }
        }

        class GreaterOrEqualArgs
        {
            [SearchArg(SearchMode.GreaterOrEqual)]
            public int? No { get; set; }
        }

        class LessArgs
        {
            [SearchArg(SearchMode.Less)]
            public int? No { get; set; }
        }

        class LessOrEqualArgs
        {
            [SearchArg(SearchMode.LessOrEqual)]
            public int? No { get; set; }
        }

        class InArgs
        {
            [SourceProperty(nameof(Student.Name))]
            [SearchArg(SearchMode.In)]
            public string[]? Names { get; set; }
        }

        class ExpressionArgs
        {
            [SearchArg(SearchMode.Expression)]
            public string? Name { get; set; }

            internal Expression<Func<Student, bool>>? NameExpr
            {
                get
                {
                    if (Name == null)
                    {
                        return null;
                    }
                    return x => x.Name.Contains(Name);
                }
            }
        }

        class FilterArgs
        {
            public IQueryable<Student> Filter(IQueryable<Student> q)
            {
                return q.Where(x => x.No == 2);
            }
        }


        #endregion

        [Fact]
        public void TestEqual()
        {
            var list = new List<Student>
            {
                new Student{ Name = "Fox", No = 1 },
                new Student{ Name = "Dog", No = 2 },
                new Student{ Name = "Cat", No = 3 },
            };
            var q = list.AsQueryable();
            EqualArgs args1 = new EqualArgs
            {
                Name = "   ",
                No = 2,
            };
            EqualArgs args2 = new EqualArgs
            {
                Name = " Dog   ",
            };
            EqualArgs args3 = new EqualArgs
            {
                Name = " Dog  ",
                No = 2,
            };

            var list1 = q.Filter(args1).ToList();
            var list2 = q.Filter(args2).ToList();
            var list3 = q.Filter(args3).ToList();

            Assert.Single(list1);
            Assert.Equal("Dog", list1[0].Name);
            Assert.Single(list2);
            Assert.Equal("Dog", list2[0].Name);
            Assert.Single(list3);
            Assert.Equal("Dog", list3[0].Name);
        }

        [Fact]
        public void TestLike()
        {
            var list = new List<Student>
            {
                new Student{ Name = "Fox", No = 1 },
                new Student{ Name = "Dog", No = 2 },
                new Student{ Name = "Cat", No = 3 },
            };
            var q = list.AsQueryable();
            LikeArgs args1 = new LikeArgs
            {
                Name = "*o*   ",
            };

            var list1 = q.Filter(args1).ToList();

            Assert.Equal(2, list1.Count);
            Assert.Equal("Fox", list1[0].Name);
            Assert.Equal("Dog", list1[1].Name);
        }

        [Fact]
        public void TestGreaterThan()
        {
            var list = new List<Student>
            {
                new Student{ Name = "Fox", No = 1 },
                new Student{ Name = "Dog", No = 2 },
                new Student{ Name = "Cat", No = 3 },
            };
            var q = list.AsQueryable();

            GreaterThanArgs args = new GreaterThanArgs
            {
                No = 1
            };

            var list1 = q.Filter(args).ToList();

            Assert.Equal(2, list1.Count);
            Assert.Equal("Dog", list1[0].Name);
            Assert.Equal("Cat", list1[1].Name);
        }

        [Fact]
        public void TestGreaterOrEqual()
        {
            var list = new List<Student>
            {
                new Student{ Name = "Fox", No = 1 },
                new Student{ Name = "Dog", No = 2 },
                new Student{ Name = "Cat", No = 3 },
            };
            var q = list.AsQueryable();

            GreaterOrEqualArgs args = new GreaterOrEqualArgs
            {
                No = 2
            };

            var list1 = q.Filter(args).ToList();

            Assert.Equal(2, list1.Count);
            Assert.Equal("Dog", list1[0].Name);
            Assert.Equal("Cat", list1[1].Name);
        }

        [Fact]
        public void TestLess()
        {
            var list = new List<Student>
            {
                new Student{ Name = "Fox", No = 1 },
                new Student{ Name = "Dog", No = 2 },
                new Student{ Name = "Cat", No = 3 },
            };
            var q = list.AsQueryable();

            LessArgs args = new LessArgs
            {
                No = 3
            };

            var list1 = q.Filter(args).ToList();

            Assert.Equal(2, list1.Count);
            Assert.Equal("Fox", list1[0].Name);
            Assert.Equal("Dog", list1[1].Name);
        }

        [Fact]
        public void TestLessOrEqual()
        {
            var list = new List<Student>
            {
                new Student{ Name = "Fox", No = 1 },
                new Student{ Name = "Dog", No = 2 },
                new Student{ Name = "Cat", No = 3 },
            };
            var q = list.AsQueryable();

            LessOrEqualArgs args = new LessOrEqualArgs
            {
                No = 2
            };

            var list1 = q.Filter(args).ToList();

            Assert.Equal(2, list1.Count);
            Assert.Equal("Fox", list1[0].Name);
            Assert.Equal("Dog", list1[1].Name);
        }

        [Fact]
        public void TestIn()
        {
            var list = new List<Student>
            {
                new Student{ Name = "Fox", No = 1 },
                new Student{ Name = "Dog", No = 2 },
                new Student{ Name = "Cat", No = 3 },
            };
            var q = list.AsQueryable();

            InArgs args = new InArgs
            {
                Names = new[] { "Fox", "Dog", "Stranger" }
            };

            var list1 = q.Filter(args).ToList();

            Assert.Equal(2, list1.Count);
            Assert.Equal("Fox", list1[0].Name);
            Assert.Equal("Dog", list1[1].Name);
        }

        [Fact]
        public void TestExpression()
        {
            var list = new List<Student>
            {
                new Student{ Name = "Fox", No = 1 },
                new Student{ Name = "Dog", No = 2 },
                new Student{ Name = "Cat", No = 3 },
            };
            var q = list.AsQueryable();

            ExpressionArgs args = new ExpressionArgs
            {
                Name = "o"
            };

            var list1 = q.Filter(args).ToList();

            Assert.Equal(2, list1.Count);
            Assert.Equal("Fox", list1[0].Name);
            Assert.Equal("Dog", list1[1].Name);
        }


        [Fact]
        public void FilterMethodOnSearchArgsShouldBeInvoked()
        {
            var list = new List<Student>
            {
                new Student{ Name = "Fox", No = 1 },
                new Student{ Name = "Dog", No = 2 },
                new Student{ Name = "Cat", No = 3 },
            };
            var q = list.AsQueryable();

            FilterArgs args = new FilterArgs();

            var list1 = q.Filter(args).ToList();

            Assert.Single(list1);
            Assert.Equal("Dog", list1[0].Name);

        }
    }
}
