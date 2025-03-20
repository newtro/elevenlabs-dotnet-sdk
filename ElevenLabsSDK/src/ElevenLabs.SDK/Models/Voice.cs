using System;
using System.Collections.Generic;

namespace ElevenLabs.SDK.Models
{
    /// <summary>
    /// Represents a voice in the ElevenLabs API.
    /// </summary>
    public class Voice
    {
        /// <summary>
        /// Gets or sets the unique identifier of the voice.
        /// </summary>
        public string VoiceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the voice.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category of the voice (e.g., "professional", "cloned").
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the labels associated with the voice (e.g., accent, age, gender).
        /// </summary>
        public Dictionary<string, string> Labels { get; set; }

        /// <summary>
        /// Gets or sets the description of the voice.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the gender of the voice.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the use case for the voice.
        /// </summary>
        public string UseCase { get; set; }

        /// <summary>
        /// Gets or sets the list of subscription tiers this voice is available for.
        /// </summary>
        public List<string> AvailableForTiers { get; set; }

        /// <summary>
        /// Gets or sets the list of models supported by this voice.
        /// </summary>
        public List<string> SupportedModels { get; set; }

        /// <summary>
        /// Gets or sets the settings for the voice.
        /// </summary>
        public VoiceSettings Settings { get; set; }
    }
}
