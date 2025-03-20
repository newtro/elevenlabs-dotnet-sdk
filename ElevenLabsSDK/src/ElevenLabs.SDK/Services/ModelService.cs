using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ElevenLabs.SDK.Client;
using ElevenLabs.SDK.Exceptions;
using ElevenLabs.SDK.Models;

namespace ElevenLabs.SDK.Services
{
    /// <summary>
    /// Implementation of the ElevenLabs Model service.
    /// </summary>
    public class ModelService : BaseService, IModelService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelService"/> class.
        /// </summary>
        /// <param name="httpClient">The ElevenLabs HTTP client.</param>
        public ModelService(IElevenLabsHttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <inheritdoc />
        public async Task<List<Model>> GetModelsAsync(CancellationToken cancellationToken = default)
        {
            return await HandleApiExceptionAsync(async () =>
            {
                var requestUri = "models";
                var response = await HttpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
                var result = await response.Content.ReadFromJsonAsync<ModelsResponse>(JsonOptions, cancellationToken).ConfigureAwait(false);
                return result?.Models ?? new List<Model>();
            }, "Failed to get models").ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Response model for the get models endpoint.
    /// </summary>
    internal class ModelsResponse
    {
        /// <summary>
        /// Gets or sets the list of models.
        /// </summary>
        public List<Model> Models { get; set; }
    }
}
