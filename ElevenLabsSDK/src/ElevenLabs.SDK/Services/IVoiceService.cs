using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ElevenLabs.SDK.Models;

namespace ElevenLabs.SDK.Services
{
    /// <summary>
    /// Interface for the ElevenLabs Voice service.
    /// </summary>
    public interface IVoiceService
    {
        /// <summary>
        /// Gets all available voices.
        /// </summary>
        /// <param name="showLegacy">Whether to include legacy premade voices in the response.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A list of available voices.</returns>
        Task<List<Voice>> GetVoicesAsync(bool showLegacy = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a specific voice by ID.
        /// </summary>
        /// <param name="voiceId">The ID of the voice to get.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>The requested voice.</returns>
        Task<Voice> GetVoiceAsync(string voiceId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the default voice settings.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>The default voice settings.</returns>
        Task<VoiceSettings> GetDefaultVoiceSettingsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the settings for a specific voice.
        /// </summary>
        /// <param name="voiceId">The ID of the voice to get settings for.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>The voice settings.</returns>
        Task<VoiceSettings> GetVoiceSettingsAsync(string voiceId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Edits the settings for a specific voice.
        /// </summary>
        /// <param name="voiceId">The ID of the voice to edit settings for.</param>
        /// <param name="settings">The new voice settings.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task EditVoiceSettingsAsync(string voiceId, VoiceSettings settings, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a specific voice.
        /// </summary>
        /// <param name="voiceId">The ID of the voice to delete.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteVoiceAsync(string voiceId, CancellationToken cancellationToken = default);
    }
}
