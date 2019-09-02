using System.Collections.Generic;
using System.Threading.Tasks;
using TPC.Api.Model;
using TPC.Api.Persistence.Repositories;

namespace TPC.Api.Features
{
    public interface IFeatureRepository : IRepository<Feature>
    {
        Task<IEnumerable<Feature>> GetAllByUserId(long userId);
        Task<IEnumerable<Feature>> GetAllByProjectId(long projectId);
        Task<bool> SetUnassignedByUserId(long userId);
        Task<bool> SetUnassignedByUserId(long userId,long projectId);
    }
}
