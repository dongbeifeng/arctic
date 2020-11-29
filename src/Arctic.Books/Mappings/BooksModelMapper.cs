using Arctic.NHibernateExtensions;
using System.Reflection;

namespace Arctic.Books.Mappings
{
    internal class BooksModelMapper : XModelMapper
    {
        public BooksModelMapper()
        {
            // 添加映射类
            this.AddMappings(Assembly.GetExecutingAssembly().GetTypes());
        }
    }
}
