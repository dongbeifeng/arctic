using Arctic.AppSeqs;
using Arctic.EventBus;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arctic.Web.Controllers
{
    /// <summary>
    /// SimpleEventBus 示例。
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        readonly EventBus.SimpleEventBus _simpleEventBus;
        readonly IAppSeqService _seqService;
        readonly ILogger _logger;

        public HelloController(EventBus.SimpleEventBus simpleEventBus, IAppSeqService seqService, ILogger logger)
        {
            _simpleEventBus = simpleEventBus;
            _seqService = seqService;
            _logger = logger;
        }

        /// <summary>
        /// 引发 Hello 事件，默认配置使用 <see cref="HelloEventHandler"/> 将事件参数写入日志。
        /// </summary>
        /// <returns>将参数原样返回。</returns>
        [HttpGet]
        [Route("fire-hello-event")]
        public async Task<string> FireHelloEventAsync(string msg)
        {
            await _simpleEventBus.FireEventAsync("Hello", msg);
            return msg;
        }

        /// <summary>
        /// 获取序列值
        /// </summary>
        /// <param name="seqName">序列名</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-next-val")]
        public async Task<int> GetNextValAsync(string seqName)
        {
            int i = await _seqService.GetNextAsync(seqName);
            return i;
        }


    }
}
