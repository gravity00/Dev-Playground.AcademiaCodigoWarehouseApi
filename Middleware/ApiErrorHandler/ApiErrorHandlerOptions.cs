using SimpleSoft.AspNetCore.Middleware;

namespace AcademiaCodigoWarehouseApi.Middleware.ApiErrorHandler {
    public class ApiErrorHandlerOptions : SimpleSoftMiddlewareOptions {
        public bool IndentJson { get; set; } = true;
    }
}