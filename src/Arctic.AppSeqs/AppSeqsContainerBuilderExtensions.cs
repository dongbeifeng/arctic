using Arctic.AppSeqs.Mappings;
using Arctic.NHibernateExtensions;
using Autofac;

namespace Arctic.AppSeqs
{
    public static class AppSeqsContainerBuilderExtensions
    {
        public static void AddAppSeqs(this ContainerBuilder builder)
        {
            builder.AddModelMapper<AppSeqsModelMapper>();
            builder.RegisterType<AppSeqService>().As<IAppSeqService>().InstancePerLifetimeScope();
        }
    }
}
