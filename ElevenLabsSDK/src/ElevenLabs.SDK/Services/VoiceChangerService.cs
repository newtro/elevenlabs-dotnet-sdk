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
    /// Implementation of the ElevenLabs Voice Changer service.
    /// </summary>
    public class VoiceChangerService : BaseService, IVoiceChangerService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VoiceChangerService"/> class.
        /// </summary>
        /// <param name="httpClient">The ElevenLabs HTTP client.</param>
        public VoiceChangerService(IElevenLabsHttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <inheritdoc />
        public async Task<Stream> ChangeVoiceAsync(Stream audioStream, string voiceId, string modelId = null, CancellationToken cancellationToken = default)
        {
            ValidateNotNull(audioStream, nameof(audioStream));
            ValidateNotNullOrWhitespace(voiceId, nameof(voiceId));

            return await HandleApiExceptionAsync(async () =>
            {
                var requestUri = $"voice-changer/convert?voice_id={voiceId}";
                if (!string.IsNullOrWhiteSpace(modelId))
                {
                    requestUri += $"&model_id={modelId}";
                }

                using var content = new MultipartFormDataContent();
                using var streamContent = new StreamContent(audioStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");
                content.Add(streamContent, "audio", "audio.mp3");

                var response = await HttpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }, $"Failed to change voice for voice ID {voiceId}").ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<Stream> StreamChangeVoiceAsync(Stream audioStream, string voiceId, string modelId = null, CancellationToken cancellationToken = default)
        {
            ValidateNotNull(audioStream, nameof(audioStream));
            ValidateNotNullOrWhitespace(voiceId, nameof(voiceId));

            return await HandleApiExceptionAsync(async () =>
            {
                var requestUri = $"voice-changer/convert/stream?voice_id={voiceId}";
                if (!string.IsNullOrWhiteSpace(modelId))
                {
                    requestUri += $"&model_id={modelId}";
                }

                using var content = new MultipartFormDataContent();
                using var streamContent = new StreamContent(audioStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");
                content.Add(streamContent, "audio", "audio.mp3");

                var response = await HttpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }, $"Failed to stream voice change for voice ID {voiceId}").ConfigureAwait(false);
        }
    }
}
