using App.Application;
using App.Application.Contracts.Persistent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Api.Filters
{
    public class NotFoundFilter<T, TId>(IGenericRepository<T, TId> genericRepository)
        : Attribute, IAsyncActionFilter where T : class where TId : struct
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var idAsObject = context.ActionArguments.TryGetValue("id", out var a);

            if (idAsObject is not TId id || await genericRepository.AnyAsync(id))
            {
                await next();
                return;
            }

            var entityName = typeof(T).Name;

            var actionName = context.ActionDescriptor.RouteValues["action"];

            var result = ApplicationResult.Fail($"data not found. ({entityName}) ({actionName}).");


            context.Result = new NotFoundObjectResult(result);

        }
    }
}
