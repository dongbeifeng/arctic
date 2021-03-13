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

using Arctic.AppSeqs;
using Arctic.EventBus;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
