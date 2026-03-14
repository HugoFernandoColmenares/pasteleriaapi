using System.Net;
using Pasteleria.Shared.DTOs;

namespace Pasteleria.Shared.Extensions
{
    /// <summary>
    /// Standardized response for all API endpoints.
    /// </summary>
    /// <typeparam name="T">Type of the data payload.</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Data payload returned from the request.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Human-readable message about the result of the request.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// List of errors encountered if the request failed.
        /// </summary>
        public List<string>? Errors { get; set; }

        /// <summary>
        /// Boolean flag indicating if the request was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// HTTP Status Code for the response.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Optional pagination metadata.
        /// </summary>
        public PaginationDto? Pagination { get; set; }

        public ApiResponse() { }

        public ApiResponse(T? data, string message = "", bool isSuccess = true, int statusCode = 200, PaginationDto? pagination = null)
        {
            Data = data;
            Message = message;
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Pagination = pagination;
        }

        public static ApiResponse<T> SuccessResponse(T? data, string message = "Request processed successfully.", int statusCode = 200, PaginationDto? pagination = null)
        {
            return new ApiResponse<T>(data, message, true, statusCode, pagination);
        }

        public static ApiResponse<T> FailureResponse(string message = "Request failed.", List<string>? errors = null, int statusCode = 400)
        {
            return new ApiResponse<T>(default, message, false, statusCode) { Errors = errors ?? new List<string>() };
        }
    }
}
