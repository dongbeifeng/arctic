using Arctic.Books;
using Arctic.NHibernateExtensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace Arctic.Web.Books
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        ISession _session;
        ILogger _logger;

        public BooksController(ISession session, ILogger logger)
        {
            _session = session;
            _logger = logger;
        }

        /// <summary>
        /// 查询图书列表
        /// </summary>
        /// <param name="args">查询参数</param>
        /// <returns></returns>
        [HttpPost]
        [DebugShowArgs]
        [AutoTransaction]
        [Route("list")]
        public async Task<BookListResult> Get(BookListArgs args)
        {
            var (list, _, _, total) = await _session.Query<Book>().ToPagedListAsync(args);
            return new BookListResult
            {
                Success = true,
                Message = "OK",                
                Data = list.Select(x => new BookListItem
                {
                    BookId = x.BookId,
                    Title = x.Title,
                    Price = x.Price,
                }),
                Total = total
            };
        }

        [HttpGet("{id}")]
        [AutoTransaction]
        public string Get(int id)
        {
            return "value";
        }

        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
