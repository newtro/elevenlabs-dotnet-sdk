using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ElevenLabs.SDK.Services
{
    /// <summary>
    /// Interface for the ElevenLabs Audio Isolation service.
    /// </summary>
    public interface IAudioIsolationService
    {
        /// <summary>
        /// Isolates voice from an audio file.
        /// </summary>
        /// <param name="audioStream">The audio stream containing the audio to process.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A stream containing the processed audio with isolated voice.</returns>
        Task<Stream> IsolateVoiceAsync(Stream audioStream, CancellationToken cancellationToken = default);

        /// <summary>
        /// Streams the voice isolation process for an audio file.
        /// </summary>
        /// <param name="audioStream">The audio stream containing the audio to process.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A stream containing the processed audio with isolated voice.</returns>
        Task<Stream> StreamIsolateVoiceAsync(Stream audioStream, CancellationToken cancellationToken = default);
    }
}
