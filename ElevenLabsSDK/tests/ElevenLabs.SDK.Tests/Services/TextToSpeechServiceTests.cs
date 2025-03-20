using System;
using System.Collections.Generic;
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
    public class TextToSpeechServiceTests
    {
        private readonly Mock<IElevenLabsHttpClient> _mockHttpClient;
        private readonly TextToSpeechService _service;

        public TextToSpeechServiceTests()
        {
            _mockHttpClient = new Mock<IElevenLabsHttpClient>();
            _service = new TextToSpeechService(_mockHttpClient.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenHttpClientIsNull()
        {
            // Arrange
            IElevenLabsHttpClient nullHttpClient = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TextToSpeechService(nullHttpClient));
        }

        [Fact]
        public async Task TextToSpeechAsync_ShouldThrowArgumentException_WhenVoiceIdIsNull()
        {
            // Arrange
            string nullVoiceId = null;
            var request = new TextToSpeechRequest { Text = "Hello world" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _service.TextToSpeechAsync(nullVoiceId, request));
        }

        [Fact]
        public async Task TextToSpeechAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            // Arrange
            string voiceId = "voice123";
            TextToSpeechRequest nullRequest = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _service.TextToSpeechAsync(voiceId, nullRequest));
        }

        [Fact]
        public async Task TextToSpeechAsync_ShouldReturnStream_WhenRequestIsSuccessful()
        {
            // Arrange
            string voiceId = "voice123";
            var request = new TextToSpeechRequest { Text = "Hello world" };
            var expectedContent = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StreamContent(expectedContent)
            };

            _mockHttpClient
                .Setup(client => client.PostAsync(
                    It.IsAny<string>(),
                    It.IsAny<HttpContent>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _service.TextToSpeechAsync(voiceId, request);

            // Assert
            Assert.NotNull(result);
            _mockHttpClient.Verify(
                client => client.PostAsync(
                    It.Is<string>(uri => uri.Contains(voiceId)),
                    It.IsAny<HttpContent>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task StreamTextToSpeechAsync_ShouldReturnStream_WhenRequestIsSuccessful()
        {
            // Arrange
            string voiceId = "voice123";
            var request = new TextToSpeechRequest { Text = "Hello world" };
            var expectedContent = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StreamContent(expectedContent)
            };

            _mockHttpClient
                .Setup(client => client.PostAsync(
                    It.IsAny<string>(),
                    It.IsAny<HttpContent>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _service.StreamTextToSpeechAsync(voiceId, request);

            // Assert
            Assert.NotNull(result);
            _mockHttpClient.Verify(
                client => client.PostAsync(
                    It.Is<string>(uri => uri.Contains(voiceId) && uri.Contains("stream")),
                    It.IsAny<HttpContent>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
