using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ElevenLabs.SDK.Client;
using ElevenLabs.SDK.Models;
using ElevenLabs.SDK.Services;
using Moq;
using Xunit;

namespace ElevenLabs.SDK.Tests.Services
{
    public class SpeechToTextServiceTests
    {
        private readonly Mock<IElevenLabsHttpClient> _mockHttpClient;
        private readonly SpeechToTextService _service;

        public SpeechToTextServiceTests()
        {
            _mockHttpClient = new Mock<IElevenLabsHttpClient>();
            _service = new SpeechToTextService(_mockHttpClient.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenHttpClientIsNull()
        {
            // Arrange
            IElevenLabsHttpClient nullHttpClient = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new SpeechToTextService(nullHttpClient));
        }

        [Fact]
        public async Task SpeechToTextAsync_ShouldThrowArgumentNullException_WhenAudioStreamIsNull()
        {
            // Arrange
            Stream nullAudioStream = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _service.SpeechToTextAsync(nullAudioStream));
        }

        [Fact]
        public async Task SpeechToTextAsync_ShouldReturnResponse_WhenRequestIsSuccessful()
        {
            // Arrange
            var audioStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            var responseObj = new SpeechToTextResponse 
            { 
                Text = "Hello world",
                Language = "en",
                Confidence = 0.95
            };
            
            var responseContent = JsonSerializer.Serialize(responseObj, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent)
            };

            _mockHttpClient
                .Setup(client => client.PostAsync(
                    It.IsAny<string>(),
                    It.IsAny<HttpContent>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _service.SpeechToTextAsync(audioStream);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Hello world", result.Text);
            Assert.Equal("en", result.Language);
            Assert.Equal(0.95, result.Confidence);
            
            _mockHttpClient.Verify(
                client => client.PostAsync(
                    It.Is<string>(uri => uri.StartsWith("speech-to-text")),
                    It.IsAny<HttpContent>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task SpeechToTextAsync_ShouldIncludeModelId_WhenProvided()
        {
            // Arrange
            var audioStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            string modelId = "model123";
            var responseObj = new SpeechToTextResponse 
            { 
                Text = "Hello world",
                Language = "en",
                Confidence = 0.95
            };
            
            var responseContent = JsonSerializer.Serialize(responseObj, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent)
            };

            _mockHttpClient
                .Setup(client => client.PostAsync(
                    It.IsAny<string>(),
                    It.IsAny<HttpContent>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _service.SpeechToTextAsync(audioStream, modelId);

            // Assert
            Assert.NotNull(result);
            
            _mockHttpClient.Verify(
                client => client.PostAsync(
                    It.Is<string>(uri => uri.Contains(modelId)),
                    It.IsAny<HttpContent>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
