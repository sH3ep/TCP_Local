using System.Collections.Generic;
using System.Threading.Tasks;
using TPC.Api.Model;
using TPC.Api.Features.Dto;

namespace TPC.Api.Features
{
    public interface IFeatureService
    {
        Task<IEnumerable<Feature>> GetFiltered(FeatureFilterDto filter);
    }
}
