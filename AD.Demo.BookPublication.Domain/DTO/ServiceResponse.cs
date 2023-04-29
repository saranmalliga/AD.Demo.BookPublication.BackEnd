using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD.Demo.BookPublication.Domain.DTO
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public string Msg { get; set; }
        public bool Success { get; set; }
    }

    public class ServiceResponse : ServiceResponse<object>
    {
        public static ServiceResponse<T> SuccessResponse<T>(string message, T payload) => new ServiceResponse<T> { Success = true, Data = payload, Msg = message };
        public static ServiceResponse<object> ErrorResponse(string message) => new ServiceResponse<object> { Msg = message, Success = false, Data = null };
        public static ServiceResponse<object> ErrorResponse(Exception exception) => ErrorResponse(exception?.Message);
    }
}
