using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ElevenLabs.SDK.Exceptions;
using Xunit;

namespace ElevenLabs.SDK.Tests.Exceptions
{
    public class ElevenLabsExceptionTests
    {
        [Fact]
        public void ElevenLabsException_ShouldSetProperties_WhenConstructed()
        {
            // Arrange
            var message = "Test error message";
            var statusCode = HttpStatusCode.BadRequest;
            var errorCode = "TEST_ERROR";
            var innerException = new Exception("Inner exception");

            // Act
            var exception = new ElevenLabsException(message, statusCode, errorCode, innerException);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(statusCode, exception.StatusCode);
            Assert.Equal(errorCode, exception.ErrorCode);
            Assert.Same(innerException, exception.InnerException);
        }

        [Fact]
        public void ElevenLabsAuthenticationException_ShouldSetProperties_WhenConstructed()
        {
            // Arrange
            var message = "Authentication failed";
            var statusCode = HttpStatusCode.Unauthorized;
            var errorCode = "AUTH_ERROR";
            var innerException = new Exception("Inner exception");

            // Act
            var exception = new ElevenLabsAuthenticationException(message, statusCode, errorCode, innerException);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(statusCode, exception.StatusCode);
            Assert.Equal(errorCode, exception.ErrorCode);
            Assert.Same(innerException, exception.InnerException);
            Assert.IsType<ElevenLabsAuthenticationException>(exception);
            Assert.IsAssignableFrom<ElevenLabsException>(exception);
        }

        [Fact]
        public void ElevenLabsResourceNotFoundException_ShouldSetProperties_WhenConstructed()
        {
            // Arrange
            var message = "Resource not found";
            var statusCode = HttpStatusCode.NotFound;
            var errorCode = "NOT_FOUND";
            var innerException = new Exception("Inner exception");

            // Act
            var exception = new ElevenLabsResourceNotFoundException(message, statusCode, errorCode, innerException);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(statusCode, exception.StatusCode);
            Assert.Equal(errorCode, exception.ErrorCode);
            Assert.Same(innerException, exception.InnerException);
            Assert.IsType<ElevenLabsResourceNotFoundException>(exception);
            Assert.IsAssignableFrom<ElevenLabsException>(exception);
        }

        [Fact]
        public void ElevenLabsValidationException_ShouldSetProperties_WhenConstructed()
        {
            // Arrange
            var message = "Validation failed";
            var statusCode = HttpStatusCode.BadRequest;
            var errorCode = "VALIDATION_ERROR";
            var innerException = new Exception("Inner exception");
            var validationErrors = new { field = "text", error = "Text is required" };

            // Act
            var exception = new ElevenLabsValidationException(message, validationErrors, statusCode, errorCode, innerException);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(statusCode, exception.StatusCode);
            Assert.Equal(errorCode, exception.ErrorCode);
            Assert.Same(innerException, exception.InnerException);
            Assert.Same(validationErrors, exception.ValidationErrors);
            Assert.IsType<ElevenLabsValidationException>(exception);
            Assert.IsAssignableFrom<ElevenLabsException>(exception);
        }

        [Fact]
        public void ElevenLabsRateLimitExceededException_ShouldSetProperties_WhenConstructed()
        {
            // Arrange
            var message = "Rate limit exceeded";
            var statusCode = HttpStatusCode.TooManyRequests;
            var errorCode = "RATE_LIMIT";
            var innerException = new Exception("Inner exception");
            var resetTime = DateTimeOffset.UtcNow.AddMinutes(5);

            // Act
            var exception = new ElevenLabsRateLimitExceededException(message, resetTime, statusCode, errorCode, innerException);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(statusCode, exception.StatusCode);
            Assert.Equal(errorCode, exception.ErrorCode);
            Assert.Same(innerException, exception.InnerException);
            Assert.Equal(resetTime, exception.ResetTime);
            Assert.IsType<ElevenLabsRateLimitExceededException>(exception);
            Assert.IsAssignableFrom<ElevenLabsException>(exception);
        }

        [Fact]
        public void ElevenLabsServerException_ShouldSetProperties_WhenConstructed()
        {
            // Arrange
            var message = "Server error";
            var statusCode = HttpStatusCode.InternalServerError;
            var errorCode = "SERVER_ERROR";
            var innerException = new Exception("Inner exception");

            // Act
            var exception = new ElevenLabsServerException(message, statusCode, errorCode, innerException);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(statusCode, exception.StatusCode);
            Assert.Equal(errorCode, exception.ErrorCode);
            Assert.Same(innerException, exception.InnerException);
            Assert.IsType<ElevenLabsServerException>(exception);
            Assert.IsAssignableFrom<ElevenLabsException>(exception);
        }
    }
}
