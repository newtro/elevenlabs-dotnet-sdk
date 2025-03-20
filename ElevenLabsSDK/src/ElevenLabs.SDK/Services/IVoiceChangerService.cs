using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ElevenLabs.SDK.Services
{
    /// <summary>
    /// Interface for the ElevenLabs Voice Changer service.
    /// </summary>
    public interface IVoiceChangerService
    {
        /// <summary>
        /// Changes the voice in an audio file.
        /// </summary>
        /// <param name="audioStream">The audio stream containing the speech to convert.</param>
        /// <param name="voiceId">The ID of the voice to use for conversion.</param>
        /// <param name="modelId">The ID of the model to use for voice changing.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A stream containing the converted audio.</returns>
        Task<Stream> ChangeVoiceAsync(Stream audioStream, string voiceId, string modelId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Streams the voice changing process for an audio file.
        /// </summary>
        /// <param name="audioStream">The audio stream containing the speech to convert.</param>
        /// <param name="voiceId">The ID of the voice to use for conversion.</param>
        /// <param name="modelId">The ID of the model to use for voice changing.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A stream containing the converted audio.</returns>
        Task<Stream> StreamChangeVoiceAsync(Stream audioStream, string voiceId, string modelId = null, CancellationToken cancellationToken = default);
    }
}
