using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ElevenLabs.SDK.Client;
using ElevenLabs.SDK.Exceptions;
using ElevenLabs.SDK.Models;

namespace ElevenLabs.SDK.Services
{
    /// <summary>
    /// Implementation of the ElevenLabs Speech-to-Text service.
    /// </summary>
    public class SpeechToTextService : BaseService, ISpeechToTextService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpeechToTextService"/> class.
        /// </summary>
        /// <param name="httpClient">The ElevenLabs HTTP client.</param>
        public SpeechToTextService(IElevenLabsHttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <inheritdoc />
        public async Task<SpeechToTextResponse> SpeechToTextAsync(Stream audioStream, string modelId = null, CancellationToken cancellationToken = default)
        {
            ValidateNotNull(audioStream, nameof(audioStream));

            return await HandleApiExceptionAsync(async () =>
            {
                var requestUri = "speech-to-text";
                if (!string.IsNullOrWhiteSpace(modelId))
                {
                    requestUri += $"?model_id={modelId}";
                }

                using var content = new MultipartFormDataContent();
                using var streamContent = new StreamContent(audioStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");
                content.Add(streamContent, "audio", "audio.mp3");

                var response = await HttpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
                return await response.Content.ReadFromJsonAsync<SpeechToTextResponse>(JsonOptions, cancellationToken).ConfigureAwait(false);
            }, "Failed to convert speech to text").ConfigureAwait(false);
        }
    }
}
