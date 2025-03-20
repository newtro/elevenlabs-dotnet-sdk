using ElevenLabs.SDK;
using ElevenLabs.SDK.Configuration;
using ElevenLabs.SDK.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ElevenLabs.SDK.Examples
{
    /// <summary>
    /// Example program demonstrating the usage of the ElevenLabs SDK.
    /// </summary>
    public class Program
    {
        static async Task Main(string[] args)
        {
            // Setup dependency injection
            var services = new ServiceCollection();

            // Add ElevenLabs services
            services.AddElevenLabs(options =>
            {
                options.ApiKey = "YOUR_API_KEY_HERE"; // Replace with your actual API key
                // Optional: customize base URL and timeout
                // options.BaseUrl = "https://api.elevenlabs.io/v1/";
                // options.TimeoutSeconds = 30;
            });

            // Build service provider
            var serviceProvider = services.BuildServiceProvider();

            // Get ElevenLabs client
            var client = serviceProvider.GetRequiredService<IElevenLabsClient>();

            try
            {
                // Example 1: Get available voices
                Console.WriteLine("Getting available voices...");
                var voices = await client.Voices.GetVoicesAsync();
                Console.WriteLine($"Found {voices.Count} voices:");
                foreach (var voice in voices)
                {
                    Console.WriteLine($"- {voice.Name} (ID: {voice.Id})");
                }
                Console.WriteLine();

                if (voices.Count > 0)
                {
                    // Example 2: Get voice settings
                    var voiceId = voices[0].Id;
                    Console.WriteLine($"Getting settings for voice: {voices[0].Name}");
                    var settings = await client.Voices.GetVoiceSettingsAsync(voiceId);
                    Console.WriteLine($"Stability: {settings.Stability}");
                    Console.WriteLine($"Similarity Boost: {settings.SimilarityBoost}");
                    Console.WriteLine();

                    // Example 3: Text-to-Speech
                    Console.WriteLine("Converting text to speech...");
                    var request = new TextToSpeechRequest
                    {
                        Text = "Hello, this is a test of the ElevenLabs text to speech API.",
                        ModelId = "eleven_monolingual_v1", // Optional: specify model ID
                        VoiceSettings = new VoiceSettings
                        {
                            Stability = 0.5,
                            SimilarityBoost = 0.75
                        }
                    };

                    using var audioStream = await client.TextToSpeech.TextToSpeechAsync(voiceId, request);
                    
                    // Save the audio to a file
                    string outputPath = "output.mp3";
                    using var fileStream = File.Create(outputPath);
                    await audioStream.CopyToAsync(fileStream);
                    Console.WriteLine($"Audio saved to {Path.GetFullPath(outputPath)}");
                    Console.WriteLine();
                }

                // Example 4: Get available models
                Console.WriteLine("Getting available models...");
                var models = await client.Models.GetModelsAsync();
                Console.WriteLine($"Found {models.Count} models:");
                foreach (var model in models)
                {
                    Console.WriteLine($"- {model.Name} (ID: {model.Id})");
                }
                Console.WriteLine();

                // Note: The following examples require audio files to be present
                // Uncomment and modify if you have appropriate audio files

                /*
                // Example 5: Speech-to-Text
                Console.WriteLine("Converting speech to text...");
                using var speechAudioStream = File.OpenRead("input_audio.mp3");
                var speechResult = await client.SpeechToText.SpeechToTextAsync(speechAudioStream);
                Console.WriteLine($"Transcribed Text: {speechResult.Text}");
                Console.WriteLine($"Language: {speechResult.Language}");
                Console.WriteLine($"Confidence: {speechResult.Confidence}");
                Console.WriteLine();

                // Example 6: Voice Changer
                Console.WriteLine("Changing voice in audio...");
                using var voiceAudioStream = File.OpenRead("input_audio.mp3");
                using var changedAudioStream = await client.VoiceChanger.ChangeVoiceAsync(
                    voiceAudioStream, 
                    voiceId, 
                    modelId: "eleven_monolingual_v1"
                );
                
                // Save the audio with changed voice
                using var changedFileStream = File.Create("voice_changed.mp3");
                await changedAudioStream.CopyToAsync(changedFileStream);
                Console.WriteLine($"Voice changed audio saved to {Path.GetFullPath("voice_changed.mp3")}");
                Console.WriteLine();

                // Example 7: Audio Isolation
                Console.WriteLine("Isolating voice from audio...");
                using var noisyAudioStream = File.OpenRead("noisy_audio.mp3");
                using var isolatedAudioStream = await client.AudioIsolation.IsolateVoiceAsync(noisyAudioStream);
                
                // Save the isolated audio
                using var isolatedFileStream = File.Create("isolated_voice.mp3");
                await isolatedAudioStream.CopyToAsync(isolatedFileStream);
                Console.WriteLine($"Isolated voice audio saved to {Path.GetFullPath("isolated_voice.mp3")}");
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner error: {ex.InnerException.Message}");
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
