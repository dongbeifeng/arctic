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

using Arctic.AspNetCore;
using Arctic.Books;
using Arctic.NHibernateExtensions;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Arctic.Web.Books
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        readonly ISession _session;
        readonly ILogger _logger;

        public BooksController(ISession session, ILogger logger)
        {
            _session = session;
            _logger = logger;
        }

        /// <summary>
        /// 列出图书
        /// </summary>
        /// <param name="args">查询参数</param>
        /// <returns></returns>
        [HttpGet("get-book-list")]
        [DebugShowArgs]
        [AutoTransaction]
        public async Task<ListData<BookListItem>> GetBookList([FromQuery]BookListArgs args)
        {
            var pagedList = await _session.Query<Book>().SearchAsync(args, args.Sort, args.Current, args.PageSize);
            return this.ListData(pagedList, x => new BookListItem
            {
                BookId = x.BookId,
                Title = x.Title,
                Price = x.Price,
            });
        }


        /// <summary>
        /// 详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-book-details/{id}")]
        [AutoTransaction]
        public async Task<ApiData<BookDetails>> GetBookDetails(int id)
        {
            var book = await _session.GetAsync<Book>(id);
            if (book == null)
            {
                throw new InvalidOperationException("NotFound");
            }

            return this.Success(new BookDetails
            {
                BookId = book.BookId,
                Author = book.Author,
                Price = book.Price,
                Title = book.Title,
            });
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost("create-book")]
        [AutoTransaction]
        public async Task<ApiData> CreateBook(CreateBookArgs args)
        {
            Book book = new Book
            {
                Author = args.Author,
                Price = args.Price,
                Title = args.Title,
                PublicationDate = args.PublicationDate ?? throw new Exception(),
            };
            await _session.SaveAsync(book);
            return this.Success();
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args"></param>
        [HttpPost("update-book/{id}")]
        [AutoTransaction]
        public async Task<ApiData> UpdateBook(int id, [FromBody] UpdateBookArgs args)
        {
            Book? book = await _session.GetAsync<Book>(id);
            if (book == null)
            {
                throw new InvalidOperationException("NotFound");
            }
            book.Author = args.Author;
            book.Price = args.Price;
            book.Title = args.Title;
            await _session.UpdateAsync(book);
            return this.Success();
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AutoTransaction]
        [HttpPost("delete-book/{id}")]
        public async Task<ApiData> Delete(int id)
        {
            Book? book = await _session.GetAsync<Book>(id);
            if (book == null)
            {
                throw new InvalidOperationException("NotFound");
            }
            if (book != null)
            {
                await _session.DeleteAsync(book);
            }
            return this.Success();
        }
    }
}
