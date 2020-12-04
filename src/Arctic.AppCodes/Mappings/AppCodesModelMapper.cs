using Arctic.NHibernateExtensions;
using System.Reflection;

namespace Arctic.AppCodes.Mappings
{
    internal class AppCodesModelMapper : XModelMapper
    {
        public AppCodesModelMapper()
        {
            // 添加映射类
            this.AddMappings(Assembly.GetExecutingAssembly().GetTypes());
        }

    }

}
