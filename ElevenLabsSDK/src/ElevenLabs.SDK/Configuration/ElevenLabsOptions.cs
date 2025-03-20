using System;

namespace ElevenLabs.SDK.Configuration
{
    /// <summary>
    /// Configuration options for the ElevenLabs API client.
    /// </summary>
    public class ElevenLabsOptions
    {
        /// <summary>
        /// The API key used for authentication with the ElevenLabs API.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// The base URL for the ElevenLabs API. Defaults to "https://api.elevenlabs.io/v1".
        /// </summary>
        public string BaseUrl { get; set; } = "https://api.elevenlabs.io/v1";

        /// <summary>
        /// Timeout for API requests in seconds. Defaults to 100 seconds.
        /// </summary>
        public int TimeoutSeconds { get; set; } = 100;

        /// <summary>
        /// Validates that the required configuration options are set.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when required options are missing or invalid.</exception>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                throw new ArgumentException("API key is required", nameof(ApiKey));
            }

            if (string.IsNullOrWhiteSpace(BaseUrl))
            {
                throw new ArgumentException("Base URL is required", nameof(BaseUrl));
            }

            if (TimeoutSeconds <= 0)
            {
                throw new ArgumentException("Timeout must be greater than 0", nameof(TimeoutSeconds));
            }
        }
    }
}
