using System;

namespace AcademiaCodigoWarehouseApi.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {

        }
    }
}