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
using ElevenLabs.SDK.Models;

namespace ElevenLabs.SDK.Services
{
    /// <summary>
    /// Interface for the ElevenLabs Text-to-Speech service.
    /// </summary>
    public interface ITextToSpeechService
    {
        /// <summary>
        /// Converts text to speech using the specified voice.
        /// </summary>
        /// <param name="voiceId">The ID of the voice to use.</param>
        /// <param name="request">The text-to-speech request.</param>
        /// <param name="outputFormat">The output format of the generated audio.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A stream containing the generated audio.</returns>
        Task<Stream> TextToSpeechAsync(string voiceId, TextToSpeechRequest request, string outputFormat = "mp3_44100_128", CancellationToken cancellationToken = default);

        /// <summary>
        /// Streams text to speech using the specified voice.
        /// </summary>
        /// <param name="voiceId">The ID of the voice to use.</param>
        /// <param name="request">The text-to-speech request.</param>
        /// <param name="outputFormat">The output format of the generated audio.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A stream containing the generated audio.</returns>
        Task<Stream> StreamTextToSpeechAsync(string voiceId, TextToSpeechRequest request, string outputFormat = "mp3_44100_128", CancellationToken cancellationToken = default);
    }
}
