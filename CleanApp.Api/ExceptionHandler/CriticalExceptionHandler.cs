using App.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CleanApp.Api.ExceptionHandler
{
    public class CriticalExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is CriticalException)
            {
                Console.WriteLine("sms has been sent about the error.");
            }

            return ValueTask.FromResult(false);
        }
    }
}
