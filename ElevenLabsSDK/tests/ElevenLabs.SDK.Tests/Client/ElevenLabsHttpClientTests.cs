using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ElevenLabs.SDK.Client;
using ElevenLabs.SDK.Configuration;
using ElevenLabs.SDK.Exceptions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;

namespace ElevenLabs.SDK.Tests.Client
{
    public class ElevenLabsHttpClientTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly Mock<IOptions<ElevenLabsOptions>> _mockOptions;
        private readonly ElevenLabsOptions _options;

        public ElevenLabsHttpClientTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _options = new ElevenLabsOptions
            {
                ApiKey = "test-api-key",
                BaseUrl = "https://api.elevenlabs.io/v1/",
                TimeoutSeconds = 30
            };
            _mockOptions = new Mock<IOptions<ElevenLabsOptions>>();
            _mockOptions.Setup(o => o.Value).Returns(_options);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenOptionsIsNull()
        {
            // Arrange
            IOptions<ElevenLabsOptions> nullOptions = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ElevenLabsHttpClient(_httpClient, nullOptions));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenHttpClientIsNull()
        {
            // Arrange
            HttpClient nullHttpClient = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ElevenLabsHttpClient(nullHttpClient, _mockOptions.Object));
        }

        [Fact]
        public void Constructor_ShouldConfigureHttpClient_WithCorrectHeaders()
        {
            // Arrange & Act
            var client = new ElevenLabsHttpClient(_httpClient, _mockOptions.Object);

            // Assert
            Assert.Equal(new Uri(_options.BaseUrl), _httpClient.BaseAddress);
            Assert.Equal(TimeSpan.FromSeconds(_options.TimeoutSeconds), _httpClient.Timeout);
            Assert.True(_httpClient.DefaultRequestHeaders.Contains("xi-api-key"));
            Assert.Equal(_options.ApiKey, _httpClient.DefaultRequestHeaders.GetValues("xi-api-key").First());
            Assert.Equal("application/json", _httpClient.DefaultRequestHeaders.Accept.First().MediaType);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnResponse_WhenRequestIsSuccessful()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"success\":true}")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var client = new ElevenLabsHttpClient(_httpClient, _mockOptions.Object);

            // Act
            var result = await client.GetAsync("test");

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith("test")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetAsync_ShouldThrowElevenLabsException_WhenRequestFails()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent("{\"status\":\"error\",\"message\":\"Invalid API key\"}")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var client = new ElevenLabsHttpClient(_httpClient, _mockOptions.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ElevenLabsAuthenticationException>(() => client.GetAsync("test"));
            Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
            Assert.Contains("Invalid API key", exception.Message);
        }

        [Fact]
        public async Task PostAsync_ShouldReturnResponse_WhenRequestIsSuccessful()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"success\":true}")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var client = new ElevenLabsHttpClient(_httpClient, _mockOptions.Object);
            var content = new StringContent("{\"test\":\"data\"}");

            // Act
            var result = await client.PostAsync("test", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri.ToString().EndsWith("test")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnResponse_WhenRequestIsSuccessful()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"success\":true}")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var client = new ElevenLabsHttpClient(_httpClient, _mockOptions.Object);

            // Act
            var result = await client.DeleteAsync("test");

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete && req.RequestUri.ToString().EndsWith("test")),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}
