using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPC.Api.Model;
using TPC.Api.Persistence;
using TPC.Api.Shared;

namespace TPC.Api.Sprints
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SprintController : CrudController<Sprint, ISprintRepository>
    {

        public SprintController(
            IUnitOfWork unitOfWork,
            ISprintRepository sprintRepository,
            IMapper mapper,
            IModificationService modificationService) :
            base(unitOfWork, sprintRepository, mapper, modificationService)
        {
        }

        [HttpPost]
        public override async Task<ActionResult<Sprint>> Post([FromBody] Sprint item)
        {
            try
            {
                return await base.Post(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{sprintId}/tasks")]
        public async Task<IEnumerable<TaskItem>> GetAllSprintTasks([FromRoute] long sprintId)
        {
            return await Repository.GetSprintTasks(sprintId);
        }

    }
}
