using System.Threading.Tasks;

namespace TPC.Api.Persistence
{
    public interface IUnitOfWork
    {
        Task<int> Complete();
    }
}
