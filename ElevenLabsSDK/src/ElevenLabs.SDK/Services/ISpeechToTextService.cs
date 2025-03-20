using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ElevenLabs.SDK.Services
{
    /// <summary>
    /// Interface for the ElevenLabs Speech-to-Text service.
    /// </summary>
    public interface ISpeechToTextService
    {
        /// <summary>
        /// Converts speech to text.
        /// </summary>
        /// <param name="audioStream">The audio stream containing the speech to convert.</param>
        /// <param name="modelId">The ID of the model to use for speech-to-text conversion.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>The speech-to-text response containing the transcribed text.</returns>
        Task<SpeechToTextResponse> SpeechToTextAsync(Stream audioStream, string modelId = null, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Response from the speech-to-text conversion.
    /// </summary>
    public class SpeechToTextResponse
    {
        /// <summary>
        /// Gets or sets the transcribed text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the ID of the model used for transcription.
        /// </summary>
        public string ModelId { get; set; }
    }
}
