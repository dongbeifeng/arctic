using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Arctic.AppCodes.Mappings
{
    internal class AppCodeMapping : ClassMapping<AppCode>
    {
        public AppCodeMapping()
        {
            Table("AppCodes");
            DynamicUpdate(true);
            Cache(cache => cache.Usage(CacheUsage.ReadWrite));

            ComposedId(id =>
            {
                id.Property(cl => cl.AppCodeType, prop => prop.Update(false));
                id.Property(cl => cl.AppCodeValue, prop => prop.Update(false));
            });

            Property(cl => cl.Description);
            Property(cl => cl.Visible);
            Property(cl => cl.Comment);
            Property(cl => cl.Scope);

            Property(cl => cl.DisplayOrder);
            Property(cl => cl.Options);
            Property(cl => cl.ex1);
            Property(cl => cl.ex2);
        }
    }

}
