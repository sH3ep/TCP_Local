using System.Threading.Tasks;

namespace TPC.Api.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(TcpContext context)
        {
            Context = context;
        }

        private TcpContext Context { get; }

        public async Task<int> Complete()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
