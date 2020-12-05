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

using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Arctic.NHibernateExtensions
{
    public static class LinqAsyncExtensionMethods
    {
        static Task<TResult> Switch<TSource, TResult>(this IQueryable<TSource> source, Func<TResult> whenMemory, Func<Task<TResult>> whenNh)
        {
            return source.Provider switch
            {
                EnumerableQuery<TSource> => Task.FromResult(whenMemory()),
                INhQueryProvider => whenNh(),
                _ => throw new NotSupportedException(),
            };
        }

        static Task<TResult?> SwitchNullable<TSource, TResult>(this IQueryable<TSource> source, Func<TResult?> whenMemory, Func<Task<TResult?>> whenNh)
        {
            return source.Provider switch
            {
                EnumerableQuery<TSource> => Task.FromResult(whenMemory()),
                INhQueryProvider => whenNh(),
                _ => throw new NotSupportedException(),
            };
        }

        public static Task<bool> AllAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.All(source, predicate),
                () => LinqExtensionMethods.AllAsync(source, predicate, cancellationToken)
            );
        }

        public static Task<bool> AnyAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Any(source),
                () => LinqExtensionMethods.AnyAsync(source, cancellationToken)
            );
        }

        public static Task<bool> AnyAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Any(source, predicate),
                () => LinqExtensionMethods.AnyAsync(source, predicate, cancellationToken)
            );
        }


        public static Task<double> AverageAsync(this IQueryable<long> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source),
                () => LinqExtensionMethods.AverageAsync(source, cancellationToken)
            );
        }

        public static Task<double?> AverageAsync(this IQueryable<long?> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source),
                () => LinqExtensionMethods.AverageAsync(source, cancellationToken)
            );
        }

        public static Task<float> AverageAsync(this IQueryable<float> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source),
                () => LinqExtensionMethods.AverageAsync(source, cancellationToken)
            );
        }

        public static Task<float?> AverageAsync(this IQueryable<float?> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source),
                () => LinqExtensionMethods.AverageAsync(source, cancellationToken)
            );
        }
        public static Task<double> AverageAsync(this IQueryable<double> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source),
                () => LinqExtensionMethods.AverageAsync(source, cancellationToken)
            );
        }
        public static Task<double?> AverageAsync(this IQueryable<double?> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source),
                () => LinqExtensionMethods.AverageAsync(source, cancellationToken)
            );
        }
        public static Task<decimal> AverageAsync(this IQueryable<decimal> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source),
                () => LinqExtensionMethods.AverageAsync(source, cancellationToken)
            );
        }
        public static Task<decimal?> AverageAsync(this IQueryable<decimal?> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source),
                () => LinqExtensionMethods.AverageAsync(source, cancellationToken)
            );
        }
        public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source, selector),
                () => LinqExtensionMethods.AverageAsync(source, selector, cancellationToken)
            );
        }
        public static Task<double?> AverageAsync(this IQueryable<int?> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source),
                () => LinqExtensionMethods.AverageAsync(source, cancellationToken)
            );
        }
        public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source, selector),
                () => LinqExtensionMethods.AverageAsync(source, selector, cancellationToken)
            );
        }
        public static Task<float> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source, selector),
                () => LinqExtensionMethods.AverageAsync(source, selector, cancellationToken)
            );
        }
        public static Task<float?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source, selector),
                () => LinqExtensionMethods.AverageAsync(source, selector, cancellationToken)
            );
        }
        public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source, selector),
                () => LinqExtensionMethods.AverageAsync(source, selector, cancellationToken)
            );
        }
        public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source, selector),
                () => LinqExtensionMethods.AverageAsync(source, selector, cancellationToken)
            );
        }
        public static Task<decimal> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source, selector),
                () => LinqExtensionMethods.AverageAsync(source, selector, cancellationToken)
            );
        }

        public static Task<decimal?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source, selector),
                () => LinqExtensionMethods.AverageAsync(source, selector, cancellationToken)
            );
        }

        public static Task<double> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source, selector),
                () => LinqExtensionMethods.AverageAsync(source, selector, cancellationToken)
            );
        }

        public static Task<double> AverageAsync(this IQueryable<int> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source),
                () => LinqExtensionMethods.AverageAsync(source, cancellationToken)
            );
        }
        public static Task<double?> AverageAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Average(source, selector),
                () => LinqExtensionMethods.AverageAsync(source, selector, cancellationToken)
            );
        }

        public static Task<int> CountAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Count(source),
                () => LinqExtensionMethods.CountAsync(source, cancellationToken)
            );
        }
        public static Task<int> CountAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.Count(source, predicate),
                () => LinqExtensionMethods.CountAsync(source, predicate, cancellationToken)
            );
        }

        public static Task<TSource> FirstAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.First(source, predicate),
                () => LinqExtensionMethods.FirstAsync(source, predicate, cancellationToken)
            );
        }

        public static Task<TSource> FirstAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.First(source),
                () => LinqExtensionMethods.FirstAsync(source, cancellationToken)
            );
        }
        public static Task<TSource?> FirstOrDefaultAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.SwitchNullable(
                () => Queryable.FirstOrDefault(source, predicate),
                () => LinqExtensionMethods.FirstOrDefaultAsync(source, predicate, cancellationToken)!
                );
        }
        public static Task<TSource?> FirstOrDefaultAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.SwitchNullable(
                () => Queryable.FirstOrDefault(source),
                () => LinqExtensionMethods.FirstOrDefaultAsync(source, cancellationToken)!
                );
        }


        public static Task<long> LongCountAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.LongCount(source, predicate),
                () => LinqExtensionMethods.LongCountAsync(source, predicate, cancellationToken));
        }
        public static Task<long> LongCountAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
                () => Queryable.LongCount(source),
                () => LinqExtensionMethods.LongCountAsync(source, cancellationToken)
                );
        }

        public static Task<TResult?> MaxAsync<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default)
        {
            return source.SwitchNullable(
                () => Queryable.Max(source, selector),
                () => LinqExtensionMethods.MaxAsync(source, selector, cancellationToken)!
                );
        }
        public static Task<TSource?> MaxAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.SwitchNullable(
                () => Queryable.Max(source),
                () => LinqExtensionMethods.MaxAsync(source, cancellationToken)!
                );
        }

        public static Task<TSource?> MinAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.SwitchNullable(
              () => Queryable.Min(source),
              () => LinqExtensionMethods.MinAsync(source, cancellationToken)!
              );
        }

        public static Task<TResult?> MinAsync<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default)
        {
            return source.SwitchNullable(
              () => Queryable.Min(source, selector),
              () => LinqExtensionMethods.MinAsync(source, selector, cancellationToken)!
              );
        }

        public static Task<TSource> SingleAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Single(source, predicate),
              () => LinqExtensionMethods.SingleAsync(source, predicate, cancellationToken));
        }

        public static Task<TSource> SingleAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Single(source),
              () => LinqExtensionMethods.SingleAsync(source, cancellationToken));
        }
        public static Task<TSource?> SingleOrDefaultAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.SingleOrDefault(source),
              () => LinqExtensionMethods.SingleOrDefaultAsync(source, cancellationToken)!
              );
        }
        public static Task<TSource?> SingleOrDefaultAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return source.SwitchNullable(
              () => Queryable.SingleOrDefault(source, predicate),
              () => LinqExtensionMethods.SingleOrDefaultAsync(source, predicate, cancellationToken)!
              );
        }
        public static Task<decimal?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source, selector),
              () => LinqExtensionMethods.SumAsync(source, selector, cancellationToken));
        }
        public static Task<double> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source, selector),
              () => LinqExtensionMethods.SumAsync(source, selector, cancellationToken));
        }
        public static Task<float?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source, selector),
              () => LinqExtensionMethods.SumAsync(source, selector, cancellationToken));
        }
        public static Task<float> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source, selector),
              () => LinqExtensionMethods.SumAsync(source, selector, cancellationToken));
        }
        public static Task<long?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source, selector),
              () => LinqExtensionMethods.SumAsync(source, selector, cancellationToken));
        }
        public static Task<long> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source, selector),
              () => LinqExtensionMethods.SumAsync(source, selector, cancellationToken));
        }
        public static Task<float?> SumAsync(this IQueryable<float?> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source),
              () => LinqExtensionMethods.SumAsync(source, cancellationToken));
        }
        public static Task<float> SumAsync(this IQueryable<float> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source),
              () => LinqExtensionMethods.SumAsync(source, cancellationToken));
        }
        public static Task<int> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source, selector),
              () => LinqExtensionMethods.SumAsync(source, selector, cancellationToken));
        }

        public static Task<long?> SumAsync(this IQueryable<long?> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source),
              () => LinqExtensionMethods.SumAsync(source, cancellationToken));
        }
        public static Task<long> SumAsync(this IQueryable<long> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source),
              () => LinqExtensionMethods.SumAsync(source, cancellationToken));
        }
        public static Task<int?> SumAsync(this IQueryable<int?> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source),
              () => LinqExtensionMethods.SumAsync(source, cancellationToken));
        }
        public static Task<int> SumAsync(this IQueryable<int> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source),
              () => LinqExtensionMethods.SumAsync(source, cancellationToken));
        }
        public static Task<int?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source, selector),
              () => LinqExtensionMethods.SumAsync(source, selector, cancellationToken));
        }
        public static Task<decimal> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source, selector),
              () => LinqExtensionMethods.SumAsync(source, selector, cancellationToken));
        }

        public static Task<double> SumAsync(this IQueryable<double> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source),
              () => LinqExtensionMethods.SumAsync(source, cancellationToken));
        }
        public static Task<double?> SumAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source, selector),
              () => LinqExtensionMethods.SumAsync(source, selector, cancellationToken));
        }

        public static Task<double?> SumAsync(this IQueryable<double?> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source),
              () => LinqExtensionMethods.SumAsync(source, cancellationToken));
        }
        public static Task<decimal> SumAsync(this IQueryable<decimal> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source),
              () => LinqExtensionMethods.SumAsync(source, cancellationToken));
        }
        public static Task<decimal?> SumAsync(this IQueryable<decimal?> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Queryable.Sum(source),
              () => LinqExtensionMethods.SumAsync(source, cancellationToken));
        }

        public static Task<List<TSource>> ToListAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.Switch(
              () => Enumerable.ToList(source),
              () => LinqExtensionMethods.ToListAsync(source, cancellationToken));
        }
    }


}
