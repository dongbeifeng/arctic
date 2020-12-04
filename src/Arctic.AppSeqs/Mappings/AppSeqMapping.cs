using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Arctic.AppSeqs.Mappings
{
    internal class AppSeqMapping : ClassMapping<AppSeq>
    {
        public AppSeqMapping()
        {
            Table("AppSeqs");
            Id(cl => cl.SeqName, id =>
            {
                id.Generator(Generators.Assigned);
                id.Length(128);
            });
            Property(cl => cl.NextVal);
        }
    }

}
