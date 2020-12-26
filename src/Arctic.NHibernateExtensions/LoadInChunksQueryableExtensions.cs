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
using System.Linq;

namespace Arctic.NHibernateExtensions
{
    /// <summary>
    /// 提供分块加载查询的方法。
    /// </summary>
    public static class LoadInChunksQueryableExtensions
    {
        /// <summary>
        /// 将查询对象分块。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        internal static async IAsyncEnumerable<List<TSource>> ToChunksAsync<TSource>(this IQueryable<TSource> source, int chunkSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (chunkSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(chunkSize), "不能小于等于 0。");
            }

            int pageIndex = -1;
            while (true)
            {
                pageIndex++;
                var page = await source.Skip(pageIndex * chunkSize).Take(chunkSize).WrappedToListAsync();
                if (page.Count == 0)
                {
                    yield break;
                }

                yield return page;

                if (page.Count < chunkSize)
                {
                    yield break;
                }
            }
        }

        /// <summary>
        /// 按指定的块大小加载查询。
        /// 这个方法是为提升分配库存操作的数据库查询效率引入的。通常有远大于需求数量的库存数据，
        /// 如果全部读取到数据库，则大量数据用不到，造成浪费，如果逐条读取，则到数据库的往返次数过多，
        /// 降低性能。这个方法允许程序以一定的块大小加载数据，从而提升性能。
        /// 调用方使用 foreach 循环遍历返回的枚举数，并指定块大小，在遇到 break 语句之前，
        /// 数据按块大小被加载进内存，遇到 break 语句后，后续的块不会加载。
        /// 以下代码以每块 10 个的大小加载数据，在遍历到第 25 个元素的时候停止，只会加载 3 个数据块，30 条数据：
        /// <code>
        ///    int count = 0;
        ///    var source = Enumerable.Range(1, 100).AsQueryable();
        ///    await foreach (var item in source.LoadInChunks(10))
        ///    {
        ///        count++;
        ///        if (count >= 25)
        ///        {
        ///            break;
        ///        }
        ///    }
        /// </code>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<TSource> LoadInChunksAsync<TSource>(this IQueryable<TSource> source, int chunkSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (chunkSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(chunkSize), "不能小于等于 0。");
            }

            await foreach (var page in source.ToChunksAsync(chunkSize))
            {
                foreach (var item in page)
                {
                    yield return item;
                }
            }
        }
    }
}
