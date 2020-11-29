using Arctic.NHibernateExtensions;
using System.Reflection;

namespace Arctic.AppSeqs.Mappings
{
    internal class AppSeqsModelMapper : XModelMapper
    {
        public AppSeqsModelMapper()
        {
            // 添加映射类
            this.AddMappings(Assembly.GetExecutingAssembly().GetTypes());
        }

    }

}
