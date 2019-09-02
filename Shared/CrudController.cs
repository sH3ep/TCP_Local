using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPC.Api.Model.Base;
using TPC.Api.Persistence;
using TPC.Api.Persistence.Repositories;

namespace TPC.Api.Shared
{
    [Authorize]
    [ApiController]
    public class CrudController<TEntity, TRepository> : ReadOnlyController<TEntity, TRepository>
        where TRepository : IRepository<TEntity>
        where TEntity : ModificationInfo, new()
    {
        public IModificationService ModificationService;
        public IUnitOfWork UnitOfWork { get; }
        public IMapper Mapper { get; }

        protected CrudController(IUnitOfWork unitOfWork, TRepository repository, IMapper mapper, IModificationService modificationService) : base(repository)
        {
            ModificationService = modificationService;
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        [HttpPost]
        public virtual async Task<ActionResult<TEntity>> Post([FromBody] TEntity item)
        {
            ModificationService.UpdateModifiedByModifiedDate(item);
            if (ModelState.IsValid)
            {
                var addedItem = Repository.Add(item);
                await UnitOfWork.Complete();
                return addedItem;
            }
            else
            {
                return BadRequest("Podano błędne dane");
            }

        }

        [HttpPut]
        public virtual async Task<ActionResult<TEntity>> Put([FromBody] TEntity item)
        {
            if (ModelState.IsValid)
            {
                if (item.Id == 0) return BadRequest();

                var itemToUpdate = await Repository.Get(item.Id);
                if (itemToUpdate == null) return NotFound();
                
                Mapper.Map(item, itemToUpdate);
                ModificationService.UpdateModifiedByModifiedDate(itemToUpdate);

                Repository.Update(itemToUpdate);

                await UnitOfWork.Complete();
                return Ok(itemToUpdate);
            }

            return BadRequest("Podano błędne dane");

        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete([FromRoute] long id)
        {
            var itemToDelete = await Repository.Get(id);

            if (itemToDelete == null) return NotFound();

            Repository.Remove(itemToDelete);
            await UnitOfWork.Complete();
            return Ok();
        }
    }
}
