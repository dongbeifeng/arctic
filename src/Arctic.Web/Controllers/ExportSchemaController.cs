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

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Arctic.Web.Controllers
{
    /// <summary>
    /// 向数据库导出表结构的工具。
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ExportSchemaController : ControllerBase
    {
        readonly Configuration _nhConfiguration;
        readonly IWebHostEnvironment _env;
        readonly ILogger _logger;

        public ExportSchemaController(Configuration nhConfiguration, IWebHostEnvironment env, ILogger logger)
        {
            _nhConfiguration = nhConfiguration;
            _env = env;
            _logger = logger;
        }

        /// <summary>
        /// 根据已注册到容器的 nh 映射信息向数据库导出表结构。此方法不是动态迁移表结构，而是删除旧表并创建新表，仅用于开发环境。
        /// </summary>
        [HttpPost]
        public async Task<string> Create()
        {
            if (_env.IsDevelopment() == false)
            {
                throw new InvalidOperationException("只能在开发环境运行此工具");
            }
            _logger.Warning("正在导出数据库架构，当前是开发环境，所有表结构将重建");
            SchemaExport export = new SchemaExport(_nhConfiguration);
            await export.CreateAsync(true, true);
            _logger.Information("已导出数据库架构");

            return "成功";
        }
    }



}
