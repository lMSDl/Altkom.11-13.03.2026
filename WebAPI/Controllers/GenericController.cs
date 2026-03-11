using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebAPI.Controllers
{
    public abstract class GenericController<T> : ApiControllerBase where T : Entity
    {
        private readonly IGenericService<T> _service;

        public GenericController(IGenericService<T> service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<T>>> Get()
        {
            return Ok(await _service.ReadAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<T>> GetById(int id)
        {
            var entity = await _service.ReadAsync(id);
            if (entity is null)
                return NotFound();

            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add(T entity)
        //public async Task<ActionResult<T>> Add(T entity)
        {
            var id = await _service.CreateAsync(entity);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, T entity)
        {
            var existing = await _service.ReadAsync(id);
            if (existing is null)
            {
                return NotFound();
            }

            await _service.UpdateAsync(id, entity);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public virtual async Task<ActionResult> Delete(int id)
        {
            var existing = await _service.ReadAsync(id);
            if (existing is null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);
            return NoContent();
        }

    }
}
