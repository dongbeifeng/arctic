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
        Configuration _nhConfiguration;
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

            SchemaExport export = new SchemaExport(_nhConfiguration);
            await export.CreateAsync(true, true);

            return "成功";
        }
    }



}
