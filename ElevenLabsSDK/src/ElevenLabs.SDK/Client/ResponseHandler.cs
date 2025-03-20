using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ElevenLabs.SDK.Exceptions;

namespace ElevenLabs.SDK.Client
{
    /// <summary>
    /// Helper class for handling HTTP responses from the ElevenLabs API.
    /// </summary>
    internal static class ResponseHandler
    {
        /// <summary>
        /// Ensures that the HTTP response was successful, or throws an appropriate exception.
        /// </summary>
        /// <param name="response">The HTTP response message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="ElevenLabsException">Thrown when the response is not successful.</exception>
        public static async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            string errorMessage = $"Request failed with status code {(int)response.StatusCode} ({response.StatusCode}): {content}";
            
            // Try to parse the error response
            ErrorResponse errorResponse = null;
            try
            {
                errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch
            {
                // Ignore deserialization errors
            }

            string errorCode = errorResponse?.Status;
            string errorDetail = errorResponse?.Message ?? content;

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    throw new ElevenLabsAuthenticationException(errorDetail, response.StatusCode, errorCode);

                case HttpStatusCode.NotFound:
                    throw new ElevenLabsResourceNotFoundException(errorDetail, response.StatusCode, errorCode);

                case HttpStatusCode.BadRequest:
                    throw new ElevenLabsValidationException(errorDetail, errorResponse?.ValidationErrors, response.StatusCode, errorCode);

                case HttpStatusCode.TooManyRequests:
                    DateTimeOffset? resetTime = null;
                    if (response.Headers.TryGetValues("X-RateLimit-Reset", out var resetValues) &&
                        DateTimeOffset.TryParse(resetValues.FirstOrDefault(), out var parsedResetTime))
                    {
                        resetTime = parsedResetTime;
                    }
                    throw new ElevenLabsRateLimitExceededException(errorDetail, resetTime, response.StatusCode, errorCode);

                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.BadGateway:
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.GatewayTimeout:
                    throw new ElevenLabsServerException(errorDetail, response.StatusCode, errorCode);

                default:
                    throw new ElevenLabsException(errorDetail, response.StatusCode, errorCode);
            }
        }
    }

    /// <summary>
    /// Represents an error response from the ElevenLabs API.
    /// </summary>
    internal class ErrorResponse
    {
        /// <summary>
        /// Gets or sets the status of the error.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the validation errors.
        /// </summary>
        public object ValidationErrors { get; set; }
    }
}
