using System;
using System.Collections.Generic;
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
    public class VoiceServiceTests
    {
        private readonly Mock<IElevenLabsHttpClient> _mockHttpClient;
        private readonly VoiceService _service;

        public VoiceServiceTests()
        {
            _mockHttpClient = new Mock<IElevenLabsHttpClient>();
            _service = new VoiceService(_mockHttpClient.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenHttpClientIsNull()
        {
            // Arrange
            IElevenLabsHttpClient nullHttpClient = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new VoiceService(nullHttpClient));
        }

        [Fact]
        public async Task GetVoicesAsync_ShouldReturnVoicesList_WhenRequestIsSuccessful()
        {
            // Arrange
            var voices = new List<Voice>
            {
                new Voice { Id = "voice1", Name = "Voice 1" },
                new Voice { Id = "voice2", Name = "Voice 2" }
            };
            
            var responseContent = JsonSerializer.Serialize(new { voices }, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent)
            };

            _mockHttpClient
                .Setup(client => client.GetAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _service.GetVoicesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("voice1", result[0].Id);
            Assert.Equal("Voice 1", result[0].Name);
            Assert.Equal("voice2", result[1].Id);
            Assert.Equal("Voice 2", result[1].Name);
            
            _mockHttpClient.Verify(
                client => client.GetAsync(
                    It.Is<string>(uri => uri.StartsWith("voices")),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetVoiceAsync_ShouldThrowArgumentException_WhenVoiceIdIsNull()
        {
            // Arrange
            string nullVoiceId = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _service.GetVoiceAsync(nullVoiceId));
        }

        [Fact]
        public async Task GetVoiceAsync_ShouldReturnVoice_WhenRequestIsSuccessful()
        {
            // Arrange
            string voiceId = "voice123";
            var voice = new Voice { Id = voiceId, Name = "Test Voice" };
            
            var responseContent = JsonSerializer.Serialize(voice, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent)
            };

            _mockHttpClient
                .Setup(client => client.GetAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _service.GetVoiceAsync(voiceId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(voiceId, result.Id);
            Assert.Equal("Test Voice", result.Name);
            
            _mockHttpClient.Verify(
                client => client.GetAsync(
                    It.Is<string>(uri => uri.Contains(voiceId)),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetDefaultVoiceSettingsAsync_ShouldReturnSettings_WhenRequestIsSuccessful()
        {
            // Arrange
            var settings = new VoiceSettings { Stability = 0.5, SimilarityBoost = 0.7 };
            
            var responseContent = JsonSerializer.Serialize(settings, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent)
            };

            _mockHttpClient
                .Setup(client => client.GetAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _service.GetDefaultVoiceSettingsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0.5, result.Stability);
            Assert.Equal(0.7, result.SimilarityBoost);
            
            _mockHttpClient.Verify(
                client => client.GetAsync(
                    It.Is<string>(uri => uri.Contains("default")),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task EditVoiceSettingsAsync_ShouldThrowArgumentException_WhenVoiceIdIsNull()
        {
            // Arrange
            string nullVoiceId = null;
            var settings = new VoiceSettings { Stability = 0.5, SimilarityBoost = 0.7 };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _service.EditVoiceSettingsAsync(nullVoiceId, settings));
        }

        [Fact]
        public async Task EditVoiceSettingsAsync_ShouldThrowArgumentNullException_WhenSettingsIsNull()
        {
            // Arrange
            string voiceId = "voice123";
            VoiceSettings nullSettings = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _service.EditVoiceSettingsAsync(voiceId, nullSettings));
        }

        [Fact]
        public async Task DeleteVoiceAsync_ShouldCallDeleteEndpoint_WhenRequestIsSuccessful()
        {
            // Arrange
            string voiceId = "voice123";
            
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };

            _mockHttpClient
                .Setup(client => client.DeleteAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpResponse);

            // Act
            await _service.DeleteVoiceAsync(voiceId);

            // Assert
            _mockHttpClient.Verify(
                client => client.DeleteAsync(
                    It.Is<string>(uri => uri.Contains(voiceId)),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
