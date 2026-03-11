using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebAPI.Controllers
{

    public class ProductsController : GenericController<Product>
    {
        public ProductsController(IGenericService<Product> service) : base(service)
        {
        }
    }
}
