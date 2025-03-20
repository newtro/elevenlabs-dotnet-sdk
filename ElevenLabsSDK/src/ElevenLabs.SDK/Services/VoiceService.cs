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
    /// Implementation of the ElevenLabs Voice service.
    /// </summary>
    public class VoiceService : BaseService, IVoiceService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VoiceService"/> class.
        /// </summary>
        /// <param name="httpClient">The ElevenLabs HTTP client.</param>
        public VoiceService(IElevenLabsHttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <inheritdoc />
        public async Task<List<Voice>> GetVoicesAsync(bool showLegacy = false, CancellationToken cancellationToken = default)
        {
            return await HandleApiExceptionAsync(async () =>
            {
                var requestUri = $"voices?show_legacy={showLegacy.ToString().ToLower()}";
                var response = await HttpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
                var result = await response.Content.ReadFromJsonAsync<VoicesResponse>(JsonOptions, cancellationToken).ConfigureAwait(false);
                return result?.Voices ?? new List<Voice>();
            }, "Failed to get voices").ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<Voice> GetVoiceAsync(string voiceId, CancellationToken cancellationToken = default)
        {
            ValidateNotNullOrWhitespace(voiceId, nameof(voiceId));

            return await HandleApiExceptionAsync(async () =>
            {
                var requestUri = $"voices/{voiceId}";
                var response = await HttpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
                return await response.Content.ReadFromJsonAsync<Voice>(JsonOptions, cancellationToken).ConfigureAwait(false);
            }, $"Failed to get voice with ID {voiceId}").ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<VoiceSettings> GetDefaultVoiceSettingsAsync(CancellationToken cancellationToken = default)
        {
            return await HandleApiExceptionAsync(async () =>
            {
                var requestUri = "voices/settings/default";
                var response = await HttpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
                return await response.Content.ReadFromJsonAsync<VoiceSettings>(JsonOptions, cancellationToken).ConfigureAwait(false);
            }, "Failed to get default voice settings").ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<VoiceSettings> GetVoiceSettingsAsync(string voiceId, CancellationToken cancellationToken = default)
        {
            ValidateNotNullOrWhitespace(voiceId, nameof(voiceId));

            return await HandleApiExceptionAsync(async () =>
            {
                var requestUri = $"voices/{voiceId}/settings";
                var response = await HttpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
                return await response.Content.ReadFromJsonAsync<VoiceSettings>(JsonOptions, cancellationToken).ConfigureAwait(false);
            }, $"Failed to get settings for voice with ID {voiceId}").ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task EditVoiceSettingsAsync(string voiceId, VoiceSettings settings, CancellationToken cancellationToken = default)
        {
            ValidateNotNullOrWhitespace(voiceId, nameof(voiceId));
            ValidateNotNull(settings, nameof(settings));

            settings.Validate();

            await HandleApiExceptionAsync(async () =>
            {
                var requestUri = $"voices/{voiceId}/settings/edit";
                var content = JsonContent.Create(settings, null, JsonOptions);
                await HttpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
                return true;
            }, $"Failed to edit settings for voice with ID {voiceId}").ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task DeleteVoiceAsync(string voiceId, CancellationToken cancellationToken = default)
        {
            ValidateNotNullOrWhitespace(voiceId, nameof(voiceId));

            await HandleApiExceptionAsync(async () =>
            {
                var requestUri = $"voices/{voiceId}";
                await HttpClient.DeleteAsync(requestUri, cancellationToken).ConfigureAwait(false);
                return true;
            }, $"Failed to delete voice with ID {voiceId}").ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Response model for the get voices endpoint.
    /// </summary>
    internal class VoicesResponse
    {
        /// <summary>
        /// Gets or sets the list of voices.
        /// </summary>
        public List<Voice> Voices { get; set; }
    }
}
