using System;
using System.Net;

namespace ElevenLabs.SDK.Exceptions
{
    /// <summary>
    /// Base exception for all ElevenLabs API exceptions.
    /// </summary>
    public class ElevenLabsException : Exception
    {
        /// <summary>
        /// Gets the HTTP status code associated with the exception.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the error code returned by the API.
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevenLabsException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="errorCode">The error code returned by the API.</param>
        /// <param name="innerException">The inner exception.</param>
        public ElevenLabsException(string message, HttpStatusCode statusCode, string errorCode = null, Exception innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }

    /// <summary>
    /// Exception thrown when authentication fails.
    /// </summary>
    public class ElevenLabsAuthenticationException : ElevenLabsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElevenLabsAuthenticationException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="errorCode">The error code returned by the API.</param>
        /// <param name="innerException">The inner exception.</param>
        public ElevenLabsAuthenticationException(string message, HttpStatusCode statusCode = HttpStatusCode.Unauthorized, string errorCode = null, Exception innerException = null)
            : base(message, statusCode, errorCode, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown when a resource is not found.
    /// </summary>
    public class ElevenLabsResourceNotFoundException : ElevenLabsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElevenLabsResourceNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="errorCode">The error code returned by the API.</param>
        /// <param name="innerException">The inner exception.</param>
        public ElevenLabsResourceNotFoundException(string message, HttpStatusCode statusCode = HttpStatusCode.NotFound, string errorCode = null, Exception innerException = null)
            : base(message, statusCode, errorCode, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown when a request is invalid.
    /// </summary>
    public class ElevenLabsValidationException : ElevenLabsException
    {
        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        public object ValidationErrors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevenLabsValidationException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="validationErrors">The validation errors.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="errorCode">The error code returned by the API.</param>
        /// <param name="innerException">The inner exception.</param>
        public ElevenLabsValidationException(string message, object validationErrors, HttpStatusCode statusCode = HttpStatusCode.BadRequest, string errorCode = null, Exception innerException = null)
            : base(message, statusCode, errorCode, innerException)
        {
            ValidationErrors = validationErrors;
        }
    }

    /// <summary>
    /// Exception thrown when the API rate limit is exceeded.
    /// </summary>
    public class ElevenLabsRateLimitExceededException : ElevenLabsException
    {
        /// <summary>
        /// Gets the time when the rate limit will reset.
        /// </summary>
        public DateTimeOffset? ResetTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevenLabsRateLimitExceededException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="resetTime">The time when the rate limit will reset.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="errorCode">The error code returned by the API.</param>
        /// <param name="innerException">The inner exception.</param>
        public ElevenLabsRateLimitExceededException(string message, DateTimeOffset? resetTime = null, HttpStatusCode statusCode = HttpStatusCode.TooManyRequests, string errorCode = null, Exception innerException = null)
            : base(message, statusCode, errorCode, innerException)
        {
            ResetTime = resetTime;
        }
    }

    /// <summary>
    /// Exception thrown when the API returns a server error.
    /// </summary>
    public class ElevenLabsServerException : ElevenLabsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElevenLabsServerException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="errorCode">The error code returned by the API.</param>
        /// <param name="innerException">The inner exception.</param>
        public ElevenLabsServerException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string errorCode = null, Exception innerException = null)
            : base(message, statusCode, errorCode, innerException)
        {
        }
    }
}
