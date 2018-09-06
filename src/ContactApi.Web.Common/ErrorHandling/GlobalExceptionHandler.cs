using System.Net;
using System.Web;
using System.Web.Http.ExceptionHandling;
using ContactApi.Data.Exceptions;

namespace ContactApi.Web.Common.ErrorHandling
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var exception = context.Exception;
            if (exception is HttpException httpException)
            {
                context.Result = new SimpleErrorResult(context.Request,
                    (HttpStatusCode)httpException.GetHttpCode(), httpException.Message);
                return;
            }

            if (exception is RootObjectNotFoundException)
            {
                context.Result = new SimpleErrorResult(context.Request, HttpStatusCode.NotFound,
                    exception.Message);
                return;
            }

            if (exception is ChildObjectNotFoundException)
            {
                context.Result = new SimpleErrorResult(context.Request, HttpStatusCode.Conflict,
                    exception.Message);
                return;
            }

            if (exception is ContactDataUpdateException)
            {
                context.Result = new SimpleErrorResult(context.Request, HttpStatusCode.BadRequest,
                    exception.Message);
                return;
            }

            context.Result = new SimpleErrorResult(context.Request, HttpStatusCode.InternalServerError,
                exception.Message);
        }
    }
}
