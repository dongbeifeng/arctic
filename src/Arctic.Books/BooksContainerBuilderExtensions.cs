using Arctic.Books.Mappings;
using Arctic.NHibernateExtensions;
using Autofac;

namespace Arctic.Books
{
    /// <summary>
    /// 用于向容器注册类型的扩展方法。在 Startup.ConfigureContainer 方法中调用。
    /// </summary>
    public static class BooksContainerBuilderExtensions
    {
        public static void AddBooks(this ContainerBuilder builder)
        {
            builder.AddModelMapper<BooksModelMapper>();
        }
    }
}
