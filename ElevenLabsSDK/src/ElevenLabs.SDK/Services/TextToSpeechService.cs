using System;
using System.Collections.Generic;
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
    /// Implementation of the ElevenLabs Text-to-Speech service.
    /// </summary>
    public class TextToSpeechService : BaseService, ITextToSpeechService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextToSpeechService"/> class.
        /// </summary>
        /// <param name="httpClient">The ElevenLabs HTTP client.</param>
        public TextToSpeechService(IElevenLabsHttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <inheritdoc />
        public async Task<Stream> TextToSpeechAsync(string voiceId, TextToSpeechRequest request, string outputFormat = "mp3_44100_128", CancellationToken cancellationToken = default)
        {
            ValidateNotNullOrWhitespace(voiceId, nameof(voiceId));
            ValidateNotNull(request, nameof(request));

            return await HandleApiExceptionAsync(async () =>
            {
                var requestUri = $"text-to-speech/{voiceId}?output_format={outputFormat}";
                var content = new StringContent(JsonSerializer.Serialize(request, JsonOptions), Encoding.UTF8, "application/json");
                
                var response = await HttpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }, $"Failed to convert text to speech for voice ID {voiceId}").ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<Stream> StreamTextToSpeechAsync(string voiceId, TextToSpeechRequest request, string outputFormat = "mp3_44100_128", CancellationToken cancellationToken = default)
        {
            ValidateNotNullOrWhitespace(voiceId, nameof(voiceId));
            ValidateNotNull(request, nameof(request));

            return await HandleApiExceptionAsync(async () =>
            {
                var requestUri = $"text-to-speech/{voiceId}/stream?output_format={outputFormat}";
                var content = new StringContent(JsonSerializer.Serialize(request, JsonOptions), Encoding.UTF8, "application/json");
                
                var response = await HttpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }, $"Failed to stream text to speech for voice ID {voiceId}").ConfigureAwait(false);
        }
    }
}
