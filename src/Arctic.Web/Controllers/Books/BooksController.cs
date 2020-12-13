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
    [Route("[controller]")]
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
        /// 图书列表
        /// </summary>
        /// <param name="args">查询参数</param>
        /// <returns></returns>
        [HttpPost]
        [DebugShowArgs]
        [AutoTransaction]
        [Route("list")]
        public async Task<BookList> List(BookListArgs args)
        {
            var pagedList = await _session.Query<Book>().SearchAsync(args, args.Sort, args.Current, args.PageSize);
            return new BookList
            {
                Success = true,
                Message = "OK",                
                Data = pagedList.List.Select(x => new BookListItem
                {
                    BookId = x.BookId,
                    Title = x.Title,
                    Price = x.Price,
                }),
                Total = pagedList.Total
            };
        }

        /// <summary>
        /// 详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AutoTransaction]
        public async Task<BookDetail> Detail(int id)
        {
            var book = await _session.GetAsync<Book>(id);
            return new BookDetail 
            {
                BookId = book.BookId,
                Author = book.Author,
                Price = book.Price,
                Title = book.Title,
            };
        }

        /// <summary>
        /// 创建图书
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        [AutoTransaction]
        [Route("create")]
        public async Task<OperationResult> Create(CreateBookArgs args)
        {
            Book book = new Book
            {
                Author = args.Author,
                Price = args.Price,
                Title = args.Title,
                PublicationDate = args.PublicationDate ?? throw new Exception(),
            };
            await _session.SaveAsync(book);
            return new OperationResult { Success = true, Message = "OK" };
        }


        /// <summary>
        /// 更新图书
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args"></param>
        [HttpPut("update/{id}")]
        [AutoTransaction]
        public async Task<OperationResult> Update(int id, [FromBody] UpdateBookArgs args)
        {
            Book? book = await _session.GetAsync<Book>(id);
            if (book == null)
            {
                throw new InvalidOperationException();
            }
            book.Author = args.Author;
            book.Price = args.Price;
            book.Title = args.Title;
            await _session.UpdateAsync(book);
            return new OperationResult { Success = true, Message = "OK" };
        }


        /// <summary>
        /// 删除图书
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AutoTransaction]
        [HttpDelete("{id}")]
        public async Task<OperationResult> Delete(int id)
        {
            Book? book = await _session.GetAsync<Book>(id);
            if (book == null)
            {
                throw new InvalidOperationException();
            }
            await _session.DeleteAsync(book);
            return new OperationResult { Success = true, Message = "OK" };

        }
    }
}
