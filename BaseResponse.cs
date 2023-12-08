using System.Net;

namespace RepharmServiceCalendar
{
    public class BaseResponse<T>
    {
        public T? Data { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; } = null!;
        public HttpStatusCode StatusCode { get; set; }

        public BaseResponse(T? data, bool hasError, string? error, HttpStatusCode statusCode)
        {
            Data = data;
            HasError = hasError;
            Error = error;
            StatusCode = statusCode;
        }
        public BaseResponse()
        {
        }

    }
}
