using Arctic.AppCodes.Mappings;
using Arctic.NHibernateExtensions;
using Autofac;

namespace Arctic.AppCodes
{
    public static class AppCodesContainerBuilderExtensions
    {
        public static void AddAppCodes(this ContainerBuilder builder)
        {
            builder.AddModelMapper<AppCodesModelMapper>();
        }
    }
}
