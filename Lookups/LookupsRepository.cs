using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TPC.Api.Model.Base;
using TPC.Api.Persistence;

namespace TPC.Api.Lookups
{
    public class LookupsRepository : ILookupsRepository
    {
        private readonly TcpContext _context;

        public LookupsRepository(TcpContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TLookupEntity>> GetAll<TLookupEntity>() where TLookupEntity : LookupEntity
        {
            var entities = _context.Set<TLookupEntity>();
            return await entities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<TLookupEntity> GetById<TLookupEntity>(long id) where TLookupEntity : LookupEntity
        {
            var entities = _context.Set<TLookupEntity>();
            var entity = await entities.FindAsync(id);
            return entity;
        }
    }
}
