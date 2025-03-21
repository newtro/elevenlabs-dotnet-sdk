using System;
using System.Collections.Generic;

namespace ElevenLabs.SDK.Models
{
    /// <summary>
    /// Represents a model in the ElevenLabs API.
    /// </summary>
    public class Model
    {
        /// <summary>
        /// Gets or sets the unique identifier of the model.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the model.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the model.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the list of languages supported by this model.
        /// </summary>
        public List<string> SupportedLanguages { get; set; }

        /// <summary>
        /// Gets or sets whether the model can perform text-to-speech conversion.
        /// </summary>
        public bool CanDoTextToSpeech { get; set; }

        /// <summary>
        /// Gets or sets whether the model can perform voice conversion.
        /// </summary>
        public bool CanDoVoiceConversion { get; set; }

        /// <summary>
        /// Gets or sets the token cost factor for using this model.
        /// </summary>
        public string TokenCostFactor { get; set; }
    }
}
