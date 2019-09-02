using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TPC.Api.Model.Lookups;

namespace TPC.Api.Lookups
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LookupsController : ControllerBase
    {
        private readonly ILookupsRepository _repository;

        public LookupsController(ILookupsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("priorities/{id}")]
        public async Task<Priority> GetPriorityById([FromRoute] long id)
        {
            return await _repository.GetById<Priority>(id);
        }

        [HttpGet("priorities")]
        public async Task<IEnumerable<Priority>> GetAllPriorities()
        {
            return await _repository.GetAll<Priority>();
        }

        [HttpGet("roles/{id}")]
        public async Task<Role> GetRoleById([FromRoute] long id)
        {
            return await _repository.GetById<Role>(id);
        }

        [HttpGet("roles")]
        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _repository.GetAll<Role>();
        }

        [HttpGet("statuses/{id}")]
        public async Task<Status> GetStatusById([FromRoute] long id)
        {
            return await _repository.GetById<Status>(id);
        }

        [HttpGet("statuses")]
        public async Task<IEnumerable<Status>> GetAllStatuses()
        {
            return await _repository.GetAll<Status>();
        }

        [HttpGet("tasktypes/{id}")]
        public async Task<TaskType> GetTaskTypeById([FromRoute] long id)
        {
            return await _repository.GetById<TaskType>(id);
        }

        [HttpGet("tasktypes")]
        public async Task<IEnumerable<TaskType>> GetAllTaskTypes()
        {
            return await _repository.GetAll<TaskType>();
        }


    }
}
