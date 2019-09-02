using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPC.Api.Model;
using TPC.Api.Persistence;
using TPC.Api.Shared;
using TPC.Api.Features.Dto;


namespace TPC.Api.Features
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FeatureController : CrudController<Feature, IFeatureRepository>
    {
        private readonly IFeatureService _featureService;

        public FeatureController(
            IUnitOfWork unitOfWork,
            IFeatureRepository FeatureRepository,
            IMapper mapper,
            IFeatureService featureService,
            IModificationService modificationService) :
                base(unitOfWork, FeatureRepository, mapper, modificationService)

        {
            _featureService = featureService;
        }

        [HttpGet("project/{projectId}")]
        public async Task<IEnumerable<Feature>> GetAllByProjectId([FromRoute] long projectId)
        {
            return await Repository.GetAllByProjectId(projectId);
        }

        [HttpGet("filtered")]
        public async Task<IEnumerable<Feature>> GetFilteredTasks([FromQuery]FeatureFilterDto featureFilterDto)
        {
            var features = await _featureService.GetFiltered(featureFilterDto);
            return features;
        }
    }
}
