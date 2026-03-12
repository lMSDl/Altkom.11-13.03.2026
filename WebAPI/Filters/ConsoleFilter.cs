using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters
{
    public class ConsoleFilter : IActionFilter //IAsyncActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("After: " + (context.Result as ObjectResult)?.StatusCode);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("Before: " + context.HttpContext.Request.Path);

        }


        /*public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
        }*/
    }
}
