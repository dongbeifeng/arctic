using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Arctic.Books.Mappings
{
    internal class BookMapping : ClassMapping<Book>
    {
        public BookMapping()
        {
            Table("Books");
            DynamicUpdate(true);
            BatchSize(10);

            Id(cl => cl.BookId, id => id.Generator(Generators.Identity));

            Property(cl => cl.ctime, prop => prop.Update(false));
            Property(cl => cl.cuser, prop => prop.Update(false));
            Property(cl => cl.mtime);
            Property(cl => cl.Title);
            Property(cl => cl.Author);
            Property(cl => cl.Price);
            Property(cl => cl.PublicationDate);
        }
    }
}
