using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebAPI.Controllers
{

    public abstract class GenericResourceController<T> : GenericController<T> where T : Entity
    {
        private readonly IGenericService<T> _service;

        public GenericResourceController(IGenericService<T> service) : base(service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<T>>> Get()
        {
            return Ok(await _service.ReadAsync());
        }


        [HttpPost]
        public async Task<ActionResult<int>> Add(T entity)
        //public async Task<ActionResult<T>> Add(T entity)
        {
            var id = await _service.CreateAsync(entity);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
    }
}
