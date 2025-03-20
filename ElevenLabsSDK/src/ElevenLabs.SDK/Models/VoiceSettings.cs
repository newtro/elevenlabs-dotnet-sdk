using System;

namespace ElevenLabs.SDK.Models
{
    /// <summary>
    /// Represents the settings for a voice in the ElevenLabs API.
    /// </summary>
    public class VoiceSettings
    {
        /// <summary>
        /// Gets or sets the stability value for the voice.
        /// Higher values result in more stable and consistent audio generation at the cost of variety.
        /// Range: 0.0 to 1.0
        /// </summary>
        public double Stability { get; set; }

        /// <summary>
        /// Gets or sets the similarity boost value for the voice.
        /// Higher values make the voice more closely match the reference audio at the cost of variety.
        /// Range: 0.0 to 1.0
        /// </summary>
        public double SimilarityBoost { get; set; }

        /// <summary>
        /// Gets or sets the style value for the voice.
        /// Higher values result in more expressive speech with more variance in tone.
        /// Range: 0.0 to 1.0
        /// </summary>
        public double Style { get; set; }

        /// <summary>
        /// Gets or sets whether to use speaker boost for the voice.
        /// Speaker boost makes the voice sound more like the reference audio.
        /// </summary>
        public bool UseSpeakerBoost { get; set; }

        /// <summary>
        /// Validates that the voice settings are within acceptable ranges.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when a setting is outside its valid range.</exception>
        public void Validate()
        {
            if (Stability < 0.0 || Stability > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(Stability), "Stability must be between 0.0 and 1.0");
            }

            if (SimilarityBoost < 0.0 || SimilarityBoost > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(SimilarityBoost), "SimilarityBoost must be between 0.0 and 1.0");
            }

            if (Style < 0.0 || Style > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(Style), "Style must be between 0.0 and 1.0");
            }
        }
    }
}
