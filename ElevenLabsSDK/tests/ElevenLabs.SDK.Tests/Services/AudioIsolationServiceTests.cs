using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ElevenLabs.SDK.Client;
using ElevenLabs.SDK.Services;
using Moq;
using Xunit;

namespace ElevenLabs.SDK.Tests.Services
{
    public class AudioIsolationServiceTests
    {
        private readonly Mock<IElevenLabsHttpClient> _mockHttpClient;
        private readonly AudioIsolationService _service;

        public AudioIsolationServiceTests()
        {
            _mockHttpClient = new Mock<IElevenLabsHttpClient>();
            _service = new AudioIsolationService(_mockHttpClient.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenHttpClientIsNull()
        {
            // Arrange
            IElevenLabsHttpClient nullHttpClient = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AudioIsolationService(nullHttpClient));
        }

        [Fact]
        public async Task IsolateVoiceAsync_ShouldThrowArgumentNullException_WhenAudioStreamIsNull()
        {
            // Arrange
            Stream nullAudioStream = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _service.IsolateVoiceAsync(nullAudioStream));
        }

        [Fact]
        public async Task IsolateVoiceAsync_ShouldReturnStream_WhenRequestIsSuccessful()
        {
            // Arrange
            var audioStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            var expectedContent = new MemoryStream(new byte[] { 5, 6, 7, 8 });
            
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
            var result = await _service.IsolateVoiceAsync(audioStream);

            // Assert
            Assert.NotNull(result);
            
            _mockHttpClient.Verify(
                client => client.PostAsync(
                    It.Is<string>(uri => uri.Contains("isolate-voice")),
                    It.IsAny<HttpContent>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task StreamIsolateVoiceAsync_ShouldThrowArgumentNullException_WhenAudioStreamIsNull()
        {
            // Arrange
            Stream nullAudioStream = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _service.StreamIsolateVoiceAsync(nullAudioStream));
        }

        [Fact]
        public async Task StreamIsolateVoiceAsync_ShouldReturnStream_WhenRequestIsSuccessful()
        {
            // Arrange
            var audioStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            var expectedContent = new MemoryStream(new byte[] { 5, 6, 7, 8 });
            
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
            var result = await _service.StreamIsolateVoiceAsync(audioStream);

            // Assert
            Assert.NotNull(result);
            
            _mockHttpClient.Verify(
                client => client.PostAsync(
                    It.Is<string>(uri => uri.Contains("isolate-voice") && uri.Contains("stream")),
                    It.IsAny<HttpContent>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
