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

using Autofac;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Logging.Serilog;
using Serilog;
using System.Collections.Generic;
using System.Security.Principal;

namespace Arctic.NHibernateExtensions
{
    public static class NHibernateContainerBuilderExtensions
    {
        static readonly ILogger _logger = Log.ForContext(typeof(NHibernateContainerBuilderExtensions));


        /// <summary>
        /// 设置 NHiberernate 使用 <see cref="SerilogLoggerFactory"/>；
        /// 使用 hibernate.cfg.xml 配置 NHibernate；
        /// 从容器解析 <see cref="XModelMapper"/> 添加到 <see cref="Configuration"/>；
        /// 根据选项添加 <see cref="CheckTransactionListener"/>；
        /// 向容器注册 <see cref="Configuration"/>；
        /// 向容器注册添加了 <see cref="AuditInterceptor"/> 的 <see cref="ISessionFactory"/>；
        /// 向容器注册 <see cref="ISession"/>；
        /// 在调用之前，应使用 <see cref="AddModelMapper{TModelMapper}(ContainerBuilder)"/> 添加 <see cref="XModelMapper"/>。
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options">配置选项。</param>
        public static void AddNHibernate(this ContainerBuilder builder)
        {
            _logger.Information("正在配置 NHibernate");

            NHibernateLogger.SetLoggersFactory(new SerilogLoggerFactory());
            _logger.Information("使用 LoggerFactory: {loggerFactory}", typeof(SerilogLoggerFactory));

            builder.Register(c =>
            {
                Configuration configuration = new Configuration();
                configuration.Configure();

                foreach (var mapper in c.Resolve<IEnumerable<XModelMapper>>())
                {
                    _logger.Information("从容器解析到 ModelMapper {modelMapperType}", mapper.GetType());
                    var mappings = mapper.CompileMappingForEachExplicitlyAddedEntity();
                    foreach (var mapping in mappings)
                    {
                        configuration.AddMapping(mapping);
                    }
                }

                // 开始：nh 事件，检查事务，要求必须打开事务
                CheckTransactionListener checkTransactionListener = new CheckTransactionListener();
                configuration.AppendListeners(ListenerType.PreInsert, new IPreInsertEventListener[] { checkTransactionListener });
                configuration.AppendListeners(ListenerType.PreUpdate, new IPreUpdateEventListener[] { checkTransactionListener });
                configuration.AppendListeners(ListenerType.PreDelete, new IPreDeleteEventListener[] { checkTransactionListener });
                configuration.AppendListeners(ListenerType.PreLoad, new IPreLoadEventListener[] { checkTransactionListener });
                // 结束：nh 事件

                _logger.Information("向 NHibernate.Cfg.Configuration 添加了事件侦听程序 CheckTransactionListener");

                return configuration;

            }).SingleInstance();

            builder.Register(c =>
            {
                // 生成 SessionFactory
                var configuration = c.Resolve<Configuration>();
                ISessionFactory sessionFactory = configuration.BuildSessionFactory();
                return sessionFactory;
            }).SingleInstance().ExternallyOwned();

            builder.Register(c => 
            {
                var principal = c.Resolve<IPrincipal>();
                AuditInterceptor interceptor = new AuditInterceptor(principal);
                return c.Resolve<ISessionFactory>()
                    .WithOptions()
                    // 添加审计拦截器
                    .Interceptor(interceptor)
                    .OpenSession();
            }
            
            ).InstancePerLifetimeScope();

            _logger.Information("已配置 NHibernate");
        }


        public static void AddModelMapper<T>(this ContainerBuilder builder) where T : XModelMapper, new()
        {
            builder.Register(c => new T()).As<XModelMapper>().SingleInstance();
            _logger.Information("向容器注册了 ModelMapper {modelMapperType}", typeof(T));
        }
    }
}