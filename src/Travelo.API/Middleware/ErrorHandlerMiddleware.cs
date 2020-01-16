using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Travelo.Core.Domain;

namespace Travelo.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(context, exception);
            }
        }

        private async static Task HandleErrorAsync(HttpContext context, Exception exception)
        {var exceptionType = exception.GetType();
            var statusCode = HttpStatusCode.InternalServerError;
            var errorCode = "error";
            switch (exception)
            {
                case DomainException e when exceptionType == typeof(DomainException):
                    errorCode = e.ErrorCode.ToString();
                    statusCode = GetHttpStatusCode(e.ErrorCode);
                    break;
                case ArgumentException e when exceptionType == typeof(ArgumentException):
                    errorCode = "invalid_parameter";
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                //etc.
            }

            var responseObject = new
            {
                message = exception.Message,
                errorCode = errorCode
            };
            var payload = JsonSerializer.Serialize(responseObject);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;
            await context.Response.WriteAsync(payload);
        }

        private static HttpStatusCode GetHttpStatusCode(DomainErrorCodes errorCodes)
        {
            switch (errorCodes)
            {
                case DomainErrorCodes.InvalidParameter:
                case DomainErrorCodes.StartDateAfterEndDate:
                case DomainErrorCodes.TripStartDateIsInThePast:
                case DomainErrorCodes.TooLongTrip:
                case DomainErrorCodes.TooShortTrip:
                case DomainErrorCodes.OverlappingTripDates:
                case DomainErrorCodes.ArgumentNullOrEmpty:
                    return HttpStatusCode.BadRequest;
                case DomainErrorCodes.CustomerAlreadyAssignedToThisTrip:
                case DomainErrorCodes.TripAlreadyExists:
                case DomainErrorCodes.CustomerAlreadyExists:
                    return HttpStatusCode.Conflict;
                case DomainErrorCodes.CustomerDoesNotExist:
                case DomainErrorCodes.TripDoesNotExists:
                    return HttpStatusCode.NotFound;
                default:
                    throw new ArgumentOutOfRangeException(nameof(errorCodes), errorCodes, null);
            }
        }
    }
}