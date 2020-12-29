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
            public string StudentName { get; init; } = default!;

            public Clazz Clazz { get; init; } = default!;

            public int No { get; init; }
        }

        class Clazz
        {
            public string ClazzName { get; init; } = default!;
        }


        class EqualArgs
        {
            [SearchArg]
            public string? StudentName { get; set; }

            [SearchArg(SearchMode.Equal)]
            public int? No { get; set; }
        }

        class LikeArgs
        {
            [SearchArg(SearchMode.Like)]
            public string? StudentName { get; set; }


            [SearchArg(SearchMode.Like)]
            [SourceProperty("Clazz.ClazzName")]
            public string? ClazzName { get; set; }
        }

        class GreaterArgs
        {
            [SearchArg(SearchMode.Greater)]
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
            [SourceProperty(nameof(Student.StudentName))]
            [SearchArg(SearchMode.In)]
            public string[]? StudentNames { get; set; }
        }

        class ExpressionArgs
        {
            [SearchArg(SearchMode.Expression)]
            public string? StudentName { get; set; }

            internal Expression<Func<Student, bool>>? StudentNameExpr
            {
                get
                {
                    if (StudentName == null)
                    {
                        return null;
                    }
                    return x => x.StudentName.Contains(StudentName);
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
                new Student{ StudentName = "Fox", No = 1 },
                new Student{ StudentName = "Dog", No = 2 },
                new Student{ StudentName = "Cat", No = 3 },
            };
            var q = list.AsQueryable();
            EqualArgs args1 = new EqualArgs
            {
                StudentName = "   ",
                No = 2,
            };
            EqualArgs args2 = new EqualArgs
            {
                StudentName = " Dog   ",
            };
            EqualArgs args3 = new EqualArgs
            {
                StudentName = " Dog  ",
                No = 2,
            };

            var list1 = q.Filter(args1).ToList();
            var list2 = q.Filter(args2).ToList();
            var list3 = q.Filter(args3).ToList();

            Assert.Single(list1);
            Assert.Equal("Dog", list1[0].StudentName);
            Assert.Single(list2);
            Assert.Equal("Dog", list2[0].StudentName);
            Assert.Single(list3);
            Assert.Equal("Dog", list3[0].StudentName);
        }

        [Fact]
        public void TestLike()
        {
            var list = new List<Student>
            {
                new Student{ StudentName = "Fox", No = 1 },
                new Student{ StudentName = "Dog", No = 2 },
                new Student{ StudentName = "Cat", No = 3 },
            };
            var q = list.AsQueryable();
            LikeArgs args1 = new LikeArgs
            {
                StudentName = "*o*   ",
            };

            var list1 = q.Filter(args1).ToList();

            Assert.Equal(2, list1.Count);
            Assert.Equal("Fox", list1[0].StudentName);
            Assert.Equal("Dog", list1[1].StudentName);
        }

        [Fact]
        public void TestLike_NavigationProperty()
        {
            var list = new List<Student>
            {
                new Student{ StudentName = "Fox", Clazz = new Clazz{ ClazzName = "canine" }, No = 1 },
                new Student{ StudentName = "Dog", Clazz = new Clazz{ ClazzName = "canine" }, No = 2 },
                new Student{ StudentName = "Cat", Clazz = new Clazz{ ClazzName = "feline" }, No = 3 },
            };
            var q = list.AsQueryable();
            LikeArgs args1 = new LikeArgs
            {
                ClazzName = "can*   ",
            };

            var list1 = q.Filter(args1).ToList();

            Assert.Equal(2, list1.Count);
            Assert.Equal("Fox", list1[0].StudentName);
            Assert.Equal("Dog", list1[1].StudentName);
        }

        [Fact]
        public void TestGreaterThan()
        {
            var list = new List<Student>
            {
                new Student{ StudentName = "Fox", No = 1 },
                new Student{ StudentName = "Dog", No = 2 },
                new Student{ StudentName = "Cat", No = 3 },
            };
            var q = list.AsQueryable();

            GreaterArgs args = new GreaterArgs
            {
                No = 1
            };

            var list1 = q.Filter(args).ToList();

            Assert.Equal(2, list1.Count);
            Assert.Equal("Dog", list1[0].StudentName);
            Assert.Equal("Cat", list1[1].StudentName);
        }

        [Fact]
        public void TestGreaterOrEqual()
        {
            var list = new List<Student>
            {
                new Student{ StudentName = "Fox", No = 1 },
                new Student{ StudentName = "Dog", No = 2 },
                new Student{ StudentName = "Cat", No = 3 },
            };
            var q = list.AsQueryable();

            GreaterOrEqualArgs args = new GreaterOrEqualArgs
            {
                No = 2
            };

            var list1 = q.Filter(args).ToList();

            Assert.Equal(2, list1.Count);
            Assert.Equal("Dog", list1[0].StudentName);
            Assert.Equal("Cat", list1[1].StudentName);
        }

        [Fact]
        public void TestLess()
        {
            var list = new List<Student>
            {
                new Student{ StudentName = "Fox", No = 1 },
                new Student{ StudentName = "Dog", No = 2 },
                new Student{ StudentName = "Cat", No = 3 },
            };
            var q = list.AsQueryable();

            LessArgs args = new LessArgs
            {
                No = 3
            };

            var list1 = q.Filter(args).ToList();

            Assert.Equal(2, list1.Count);
            Assert.Equal("Fox", list1[0].StudentName);
            Assert.Equal("Dog", list1[1].StudentName);
        }

        [Fact]
        public void TestLessOrEqual()
        {
            var list = new List<Student>
            {
                new Student{ StudentName = "Fox", No = 1 },
                new Student{ StudentName = "Dog", No = 2 },
                new Student{ StudentName = "Cat", No = 3 },
            };
            var q = list.AsQueryable();

            LessOrEqualArgs args = new LessOrEqualArgs
            {
                No = 2
            };

            var list1 = q.Filter(args).ToList();

            Assert.Equal(2, list1.Count);
            Assert.Equal("Fox", list1[0].StudentName);
            Assert.Equal("Dog", list1[1].StudentName);
        }

        [Fact]
        public void TestIn()
        {
            var list = new List<Student>
            {
                new Student{ StudentName = "Fox", No = 1 },
                new Student{ StudentName = "Dog", No = 2 },
                new Student{ StudentName = "Cat", No = 3 },
            };
            var q = list.AsQueryable();

            InArgs args = new InArgs
            {
                StudentNames = new[] { "Fox", "Dog", "Stranger" }
            };

            var list1 = q.Filter(args).ToList();

            Assert.Equal(2, list1.Count);
            Assert.Equal("Fox", list1[0].StudentName);
            Assert.Equal("Dog", list1[1].StudentName);
        }

        [Fact]
        public void TestExpression()
        {
            var list = new List<Student>
            {
                new Student{ StudentName = "Fox", No = 1 },
                new Student{ StudentName = "Dog", No = 2 },
                new Student{ StudentName = "Cat", No = 3 },
            };
            var q = list.AsQueryable();

            ExpressionArgs args = new ExpressionArgs
            {
                StudentName = "o"
            };

            var list1 = q.Filter(args).ToList();

            Assert.Equal(2, list1.Count);
            Assert.Equal("Fox", list1[0].StudentName);
            Assert.Equal("Dog", list1[1].StudentName);
        }


        [Fact]
        public void FilterMethodOnSearchArgsShouldBeInvoked()
        {
            var list = new List<Student>
            {
                new Student{ StudentName = "Fox", No = 1 },
                new Student{ StudentName = "Dog", No = 2 },
                new Student{ StudentName = "Cat", No = 3 },
            };
            var q = list.AsQueryable();

            FilterArgs args = new FilterArgs();

            var list1 = q.Filter(args).ToList();

            Assert.Single(list1);
            Assert.Equal("Dog", list1[0].StudentName);

        }
    }
}
