using System.Collections.Generic;

namespace ElevenLabs.SDK.Models
{
    /// <summary>
    /// Represents a text-to-speech request in the ElevenLabs API.
    /// </summary>
    public class TextToSpeechRequest
    {
        /// <summary>
        /// Gets or sets the text that will be converted into speech.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the model to use for text-to-speech conversion.
        /// </summary>
        public string ModelId { get; set; } = "eleven_monolingual_v1";

        /// <summary>
        /// Gets or sets the language code (ISO 639-1) used to enforce a language for the model.
        /// </summary>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the voice settings to use for the text-to-speech conversion.
        /// </summary>
        public VoiceSettings VoiceSettings { get; set; }

        /// <summary>
        /// Gets or sets the pronunciation dictionary locators to apply to the text.
        /// </summary>
        public List<PronunciationDictionaryLocator> PronunciationDictionaryLocators { get; set; }

        /// <summary>
        /// Gets or sets the seed for deterministic sampling.
        /// </summary>
        public int? Seed { get; set; }

        /// <summary>
        /// Gets or sets the text that came before the text of the current request.
        /// </summary>
        public string PreviousText { get; set; }

        /// <summary>
        /// Gets or sets the text that comes after the text of the current request.
        /// </summary>
        public string NextText { get; set; }

        /// <summary>
        /// Gets or sets the list of request IDs of the samples that were generated before this generation.
        /// </summary>
        public List<string> PreviousRequestIds { get; set; }

        /// <summary>
        /// Gets or sets the list of request IDs of the samples that come after this generation.
        /// </summary>
        public List<string> NextRequestIds { get; set; }

        /// <summary>
        /// Gets or sets the text normalization mode.
        /// </summary>
        public string ApplyTextNormalization { get; set; } = "auto";
    }
}
