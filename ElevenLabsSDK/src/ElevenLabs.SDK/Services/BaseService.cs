using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ElevenLabs.SDK.Client;
using ElevenLabs.SDK.Exceptions;
using ElevenLabs.SDK.Models;

namespace ElevenLabs.SDK.Services
{
    /// <summary>
    /// Base class for ElevenLabs services that provides common functionality.
    /// </summary>
    internal abstract class BaseService
    {
        protected readonly IElevenLabsHttpClient HttpClient;
        protected static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService"/> class.
        /// </summary>
        /// <param name="httpClient">The ElevenLabs HTTP client.</param>
        protected BaseService(IElevenLabsHttpClient httpClient)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Validates that a parameter is not null or whitespace.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <exception cref="ArgumentException">Thrown when the value is null or whitespace.</exception>
        protected static void ValidateNotNullOrWhitespace(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{paramName} cannot be null or whitespace.", paramName);
            }
        }

        /// <summary>
        /// Validates that a parameter is not null.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
        protected static void ValidateNotNull(object value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Handles exceptions that may occur during API calls.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="apiCall">The API call to execute.</param>
        /// <param name="errorMessage">The error message to use if an exception occurs.</param>
        /// <returns>The result of the API call.</returns>
        protected static async Task<T> HandleApiExceptionAsync<T>(Func<Task<T>> apiCall, string errorMessage)
        {
            try
            {
                return await apiCall().ConfigureAwait(false);
            }
            catch (ElevenLabsException)
            {
                // Re-throw ElevenLabsExceptions as they already contain appropriate error information
                throw;
            }
            catch (Exception ex)
            {
                // Wrap other exceptions in an ElevenLabsException
                throw new ElevenLabsException(errorMessage, System.Net.HttpStatusCode.InternalServerError, null, ex);
            }
        }
    }
}
