using System.Collections.Generic;
using System.Threading.Tasks;
using TPC.Api.Model.Base;

namespace TPC.Api.Lookups
{
    public interface ILookupsRepository
    {
        Task<IEnumerable<TLookupEntity>> GetAll<TLookupEntity>() where TLookupEntity : LookupEntity;

        Task<TLookupEntity> GetById<TLookupEntity>(long id) where TLookupEntity : LookupEntity;
    }
}
