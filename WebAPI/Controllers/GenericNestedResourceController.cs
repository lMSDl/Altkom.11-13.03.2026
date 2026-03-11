using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebAPI.Controllers
{

    public abstract class GenericNestedResourceController<T, TParent> : ApiControllerBase
    {
        private readonly IGenericService<TParent> _parentService;
        private readonly IGenericService<T> _service;

        protected GenericNestedResourceController(IGenericService<TParent> parentService, IGenericService<T> service)
        {
            _parentService = parentService;
            _service = service;
        }


        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<T>>> Get(int parentId)
        {
            var parentEntity = await _parentService.ReadAsync(parentId);
            if (parentEntity == null)
            {
                return NotFound($"Parent entity with ID {parentId} not found.");
            }

            var result = await _service.ReadAsync(x => GetParentId(x) == parentId);

            return Ok(result);
        }

        protected abstract int GetParentId(T entity);
        protected abstract void SetParentId(T entity, int parentId);

        [HttpPost]
        public async Task<ActionResult<int>> Add(int parentId, T entity)
        {
            var parentEntity = await _parentService.ReadAsync(parentId);
            if (parentEntity == null)
            {
                return NotFound($"Parent entity with ID {parentId} not found.");
            }

            SetParentId(entity, parentId);
            var id = await _service.CreateAsync(entity);

            return CreatedAtAction(id);
        }

        protected abstract ActionResult<int> CreatedAtAction(int id);
    }
}
