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
    public class ModelServiceTests
    {
        private readonly Mock<IElevenLabsHttpClient> _mockHttpClient;
        private readonly ModelService _service;

        public ModelServiceTests()
        {
            _mockHttpClient = new Mock<IElevenLabsHttpClient>();
            _service = new ModelService(_mockHttpClient.Object);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenHttpClientIsNull()
        {
            // Arrange
            IElevenLabsHttpClient nullHttpClient = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ModelService(nullHttpClient));
        }

        [Fact]
        public async Task GetModelsAsync_ShouldReturnModelsList_WhenRequestIsSuccessful()
        {
            // Arrange
            var models = new List<Model>
            {
                new Model { Id = "model1", Name = "Model 1" },
                new Model { Id = "model2", Name = "Model 2" }
            };
            
            var responseContent = JsonSerializer.Serialize(new { models }, new JsonSerializerOptions
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
            var result = await _service.GetModelsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("model1", result[0].Id);
            Assert.Equal("Model 1", result[0].Name);
            Assert.Equal("model2", result[1].Id);
            Assert.Equal("Model 2", result[1].Name);
            
            _mockHttpClient.Verify(
                client => client.GetAsync(
                    It.Is<string>(uri => uri == "models"),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
