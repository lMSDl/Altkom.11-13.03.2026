using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebAPI.Controllers
{

    public abstract class GenericResourceController<T> : GenericController<T> where T : Entity
    {
        private readonly IGenericService<T> _service;
        private readonly IValidator<T>? _validator;


        public GenericResourceController(IGenericService<T> service, IValidator<T>? validator = null) : base(service)
        {
            _service = service;
            _validator = validator;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<T>>> Get()
        {
            return Ok(await _service.ReadAsync());
        }


        [HttpPost]
        public virtual async Task<ActionResult<int>> Add(T entity)
        //public async Task<ActionResult<T>> Add(T entity)
        {
            if(_validator is not null)
            {
                var validationResult = await _validator.ValidateAsync(entity);
                if (!validationResult.IsValid)
                {
                    /*foreach (var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }*/
                    return BadRequest(validationResult.ToDictionary());
                }
            }

            //ręcznie sprawdzenie czy model jest poprawny, ponieważ [ApiController] automatycznie zwraca 400 Bad Request, jeśli model jest niepoprawny, ale w tym przypadku chcemy zwrócić 400 z informacją o błędach walidacji
           /* if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/


            var id = await _service.CreateAsync(entity);

            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
    }
}
