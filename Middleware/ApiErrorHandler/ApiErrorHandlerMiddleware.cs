using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcademiaCodigoWarehouseApi.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimpleSoft.AspNetCore.Middleware;

namespace AcademiaCodigoWarehouseApi.Middleware.ApiErrorHandler {
    public partial class ApiErrorHandlerMiddleware : SimpleSoftMiddleware {

        public ApiErrorHandlerMiddleware (RequestDelegate next,
            IOptions<ApiErrorHandlerOptions> options, ILogger<ApiErrorHandlerMiddleware> logger = null) : base (next, options, logger) {
            Options = options?.Value ?? throw new ArgumentNullException (nameof (options));
        }

        protected new ApiErrorHandlerOptions Options { get; }

        protected override async Task OnInvoke (HttpContext context) {

            int statusCode;
            string message;
            IDictionary<string,IList<string>> modelState;

            try {
                await Next (context).ConfigureAwait (false);

                Logger.LogDebug ("No exceptions have been thrown");
                return;
            } catch (NotFoundException e) {
                Logger.LogDebug (e, "Not found exception has been thrown");

                statusCode = StatusCodes.Status404NotFound;
                message = "Endereço não encontrado";
                modelState = new Dictionary<string, IList<string>>(0);

            } catch (ValidationException e) {
                Logger.LogInformation (e, "Validation exception has been thrown");

                statusCode = StatusCodes.Status422UnprocessableEntity;
                message = "Dados inválidos";
                modelState = e.Data;

            } catch (BusinessException e) {
                Logger.LogWarning (e, "Business exception has been thrown");

                statusCode = StatusCodes.Status409Conflict;
                message = e.Message;
                modelState = new Dictionary<string, IList<string>>(0);

            } catch (Exception e) {
                Logger.LogError (e, "Unhandled exception has been thrown");

                statusCode = StatusCodes.Status500InternalServerError;
                message = "Erro interno do servidor";
                modelState = new Dictionary<string, IList<string>>(0);
            }

            context.Response.Clear ();
            context.Response.StatusCode = statusCode;
            await context.Response.WriteJsonAsync (new {
                Code = statusCode,
                Message = message,
                ModelState=modelState
            }, Options.IndentJson);
        }
    }
}