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

namespace Arctic.Web.Books
{
    /// <summary>
    /// 图书列表页的数据项。
    /// </summary>
    public class BookListItem
    {
        /// <summary>
        /// 图书 Id。
        /// </summary>
        public int BookId { get; init; }

        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; init; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; init; }

    }


}
