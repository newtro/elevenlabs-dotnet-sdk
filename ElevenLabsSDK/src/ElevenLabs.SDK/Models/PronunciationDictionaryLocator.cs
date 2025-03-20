namespace ElevenLabs.SDK.Models
{
    /// <summary>
    /// Represents a pronunciation dictionary locator in the ElevenLabs API.
    /// </summary>
    public class PronunciationDictionaryLocator
    {
        /// <summary>
        /// Gets or sets the unique identifier of the pronunciation dictionary.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the version identifier of the pronunciation dictionary.
        /// </summary>
        public string VersionId { get; set; }
    }
}
