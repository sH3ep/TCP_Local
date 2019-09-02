using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TPC.Api.Model.Base;
using TPC.Api.Persistence.Repositories;

namespace TPC.Api.Shared
{
    public class ReadOnlyController<TEntity, TRepository> : Controller
        where TRepository : IRepository<TEntity>
        where TEntity : ModificationInfo, new()
    {
        public TRepository Repository { get; set; }

        public ReadOnlyController(TRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetAll()
        {
            var result = await Repository.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TEntity>> Get([FromRoute] int id)
        {
            var item = await Repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
    }
}
