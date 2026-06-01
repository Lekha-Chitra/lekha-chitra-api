using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Response
{
    public class ServiceResponse
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;

        public static ServiceResponse BadRequest(string? message = null)
        {
            return new ServiceResponse
            {
                IsSuccess = false,
                Message = message ?? "Bad Request",
                StatusCode = 400,
            };
        }

        public static ServiceResponse Unauthorized(string? message = null)
        {
            return new ServiceResponse
            {
                IsSuccess = false,
                Message = message ?? "Unauthorized Request",
                StatusCode = 401,
            };
        }
        public static ServiceResponse Forbidden(string? message = null)
        {
            return new ServiceResponse
            {
                IsSuccess = false,
                Message = message ?? "Forbidden Request",
                StatusCode = 403,
            };
        }
        public static ServiceResponse NotFound(string? message = null)
        {
            return new ServiceResponse
            {
                IsSuccess = false,
                Message = message ?? "Not Found",
                StatusCode = 404,
            };
        }
        public static ServiceResponse Conflict(string? message = null)
        {
            return new ServiceResponse
            {
                IsSuccess = false,
                Message = message ?? "Conflicting Request",
                StatusCode = 409,
            };
        }
        public static ServiceResponse Locked(string? message = null)
        {
            return new ServiceResponse
            {
                IsSuccess = false,
                Message = message ?? "Resource Locked",
                StatusCode = 423,
            };
        }
        public static ServiceResponse InternalServerError(string? message = null)
        {
            return new ServiceResponse
            {
                IsSuccess = false,
                Message = message ?? "Internal Server Error",
                StatusCode = 500,
            };
        }
        public static ServiceResponse Success(string? message = null)
        {
            return new ServiceResponse
            {
                IsSuccess = true,
                Message = message ?? "Operation Successful",
                StatusCode = 200,
            };
        }



    }

    public class ServiceResponse<T> : ServiceResponse
        where T : class
    {
        public T? Data { get; set; }

        public static ServiceResponse<T> Success(T? data = null, string? message = null)
        {
            return new ServiceResponse<T>
            {
                IsSuccess = true,
                Message = message ?? "Operation Successful",
                StatusCode = 200,
                Data = data
            };
        }
        public static ServiceResponse<T> Created(T? data = null, string? message = null)
        {
            return new ServiceResponse<T>
            {
                IsSuccess = true,
                Message = message ?? "Operation Successful",
                StatusCode = 201,
                Data = data
            };
        }
        public static ServiceResponse<T> BadRequest(string? message = null)
        {
            return new ServiceResponse<T>
            {
                IsSuccess = false,
                Message = message ?? "Bad Request",
                StatusCode = 400,
            };
        }

        public static ServiceResponse<T> Unauthorized(string? message = null)
        {
            return new ServiceResponse<T>
            {
                IsSuccess = false,
                Message = message ?? "Unauthorized Request",
                StatusCode = 401,
            };
        }
        public static ServiceResponse<T> Forbidden(string? message = null)
        {
            return new ServiceResponse<T>
            {
                IsSuccess = false,
                Message = message ?? "Forbidden Request",
                StatusCode = 403,
            };
        }

    }
}
