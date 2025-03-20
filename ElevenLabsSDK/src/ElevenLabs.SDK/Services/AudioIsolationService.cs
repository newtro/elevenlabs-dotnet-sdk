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

namespace ElevenLabs.SDK.Services
{
    /// <summary>
    /// Implementation of the ElevenLabs Audio Isolation service.
    /// </summary>
    public class AudioIsolationService : BaseService, IAudioIsolationService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioIsolationService"/> class.
        /// </summary>
        /// <param name="httpClient">The ElevenLabs HTTP client.</param>
        public AudioIsolationService(IElevenLabsHttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <inheritdoc />
        public async Task<Stream> IsolateVoiceAsync(Stream audioStream, CancellationToken cancellationToken = default)
        {
            ValidateNotNull(audioStream, nameof(audioStream));

            return await HandleApiExceptionAsync(async () =>
            {
                var requestUri = "audio-isolation/isolate-voice";

                using var content = new MultipartFormDataContent();
                using var streamContent = new StreamContent(audioStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");
                content.Add(streamContent, "audio", "audio.mp3");

                var response = await HttpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }, "Failed to isolate voice from audio").ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<Stream> StreamIsolateVoiceAsync(Stream audioStream, CancellationToken cancellationToken = default)
        {
            ValidateNotNull(audioStream, nameof(audioStream));

            return await HandleApiExceptionAsync(async () =>
            {
                var requestUri = "audio-isolation/isolate-voice/stream";

                using var content = new MultipartFormDataContent();
                using var streamContent = new StreamContent(audioStream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");
                content.Add(streamContent, "audio", "audio.mp3");

                var response = await HttpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }, "Failed to stream voice isolation from audio").ConfigureAwait(false);
        }
    }
}
