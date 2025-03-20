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
    public class VoiceChangerServiceTests
    {
        private readonly Mock<IElevenLabsHttpClient> _mockHttpClient;
        private readonly VoiceChangerService _service;

        public VoiceChangerServiceTests()
        {
            _mockHttpClient = new Mock<IElevenLabsHttpClient>();
            _service = new VoiceChangerService(_mockHttpClient.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenHttpClientIsNull()
        {
            // Arrange
            IElevenLabsHttpClient nullHttpClient = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new VoiceChangerService(nullHttpClient));
        }

        [Fact]
        public async Task ChangeVoiceAsync_ShouldThrowArgumentNullException_WhenAudioStreamIsNull()
        {
            // Arrange
            Stream nullAudioStream = null;
            string voiceId = "voice123";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _service.ChangeVoiceAsync(nullAudioStream, voiceId));
        }

        [Fact]
        public async Task ChangeVoiceAsync_ShouldThrowArgumentException_WhenVoiceIdIsNull()
        {
            // Arrange
            var audioStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            string nullVoiceId = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _service.ChangeVoiceAsync(audioStream, nullVoiceId));
        }

        [Fact]
        public async Task ChangeVoiceAsync_ShouldReturnStream_WhenRequestIsSuccessful()
        {
            // Arrange
            var audioStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            string voiceId = "voice123";
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
            var result = await _service.ChangeVoiceAsync(audioStream, voiceId);

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
        public async Task ChangeVoiceAsync_ShouldIncludeModelId_WhenProvided()
        {
            // Arrange
            var audioStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            string voiceId = "voice123";
            string modelId = "model123";
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
            var result = await _service.ChangeVoiceAsync(audioStream, voiceId, modelId);

            // Assert
            Assert.NotNull(result);
            
            _mockHttpClient.Verify(
                client => client.PostAsync(
                    It.Is<string>(uri => uri.Contains(voiceId) && uri.Contains(modelId)),
                    It.IsAny<HttpContent>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task StreamChangeVoiceAsync_ShouldReturnStream_WhenRequestIsSuccessful()
        {
            // Arrange
            var audioStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            string voiceId = "voice123";
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
            var result = await _service.StreamChangeVoiceAsync(audioStream, voiceId);

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
