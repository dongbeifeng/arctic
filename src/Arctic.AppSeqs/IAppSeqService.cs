using System.Threading.Tasks;

namespace Arctic.AppSeqs
{
    public interface IAppSeqService
    {
        Task<int> GetNextAsync(string seqName);
    }
}
