# ElevenLabs .NET SDK Documentation

## Overview

The ElevenLabs .NET SDK provides a comprehensive client library for integrating with the ElevenLabs API in .NET applications. This SDK supports all ElevenLabs API features including text-to-speech, voice management, speech-to-text, voice changing, and audio isolation.

## Installation

Install the ElevenLabs SDK via NuGet Package Manager:

```bash
dotnet add package ElevenLabs.SDK
```

Or via the Package Manager Console:

```powershell
Install-Package ElevenLabs.SDK
```

## Getting Started

### Configuration

To use the ElevenLabs SDK, you need to configure it with your API key:

```csharp
using ElevenLabs.SDK;
using ElevenLabs.SDK.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Setup dependency injection
var services = new ServiceCollection();

// Add ElevenLabs services
services.AddElevenLabs(options =>
{
    options.ApiKey = "your-api-key-here";
    // Optional: customize base URL and timeout
    // options.BaseUrl = "https://api.elevenlabs.io/v1/";
    // options.TimeoutSeconds = 30;
});

// Build service provider
var serviceProvider = services.BuildServiceProvider();

// Get ElevenLabs client
var elevenLabsClient = serviceProvider.GetRequiredService<IElevenLabsClient>();
```

### Text-to-Speech Example

Convert text to speech using a specific voice:

```csharp
using ElevenLabs.SDK.Models;
using System.IO;
using System.Threading.Tasks;

public async Task TextToSpeechExample(IElevenLabsClient client)
{
    // Create a text-to-speech request
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

    // Get available voices
    var voices = await client.Voices.GetVoicesAsync();
    var voiceId = voices[0].Id; // Use the first available voice

    // Convert text to speech
    using var audioStream = await client.TextToSpeech.TextToSpeechAsync(voiceId, request);
    
    // Save the audio to a file
    using var fileStream = File.Create("output.mp3");
    await audioStream.CopyToAsync(fileStream);
}
```

### Streaming Text-to-Speech Example

Stream text-to-speech conversion for real-time audio:

```csharp
public async Task StreamTextToSpeechExample(IElevenLabsClient client)
{
    var request = new TextToSpeechRequest
    {
        Text = "This is a streaming text to speech example.",
        ModelId = "eleven_monolingual_v1"
    };

    var voices = await client.Voices.GetVoicesAsync();
    var voiceId = voices[0].Id;

    // Stream text to speech
    using var audioStream = await client.TextToSpeech.StreamTextToSpeechAsync(voiceId, request);
    
    // Process the stream in real-time
    // For example, play it through an audio player or save it
    using var fileStream = File.Create("streaming_output.mp3");
    await audioStream.CopyToAsync(fileStream);
}
```

## Voice Management

### Get Available Voices

Retrieve all available voices:

```csharp
public async Task GetVoicesExample(IElevenLabsClient client)
{
    var voices = await client.Voices.GetVoicesAsync();
    
    foreach (var voice in voices)
    {
        Console.WriteLine($"Voice ID: {voice.Id}");
        Console.WriteLine($"Name: {voice.Name}");
        Console.WriteLine($"Category: {voice.Category}");
        Console.WriteLine();
    }
}
```

### Get Voice Settings

Retrieve settings for a specific voice:

```csharp
public async Task GetVoiceSettingsExample(IElevenLabsClient client, string voiceId)
{
    var settings = await client.Voices.GetVoiceSettingsAsync(voiceId);
    
    Console.WriteLine($"Stability: {settings.Stability}");
    Console.WriteLine($"Similarity Boost: {settings.SimilarityBoost}");
}
```

### Edit Voice Settings

Modify settings for a specific voice:

```csharp
public async Task EditVoiceSettingsExample(IElevenLabsClient client, string voiceId)
{
    var settings = new VoiceSettings
    {
        Stability = 0.8,
        SimilarityBoost = 0.6
    };
    
    await client.Voices.EditVoiceSettingsAsync(voiceId, settings);
    Console.WriteLine("Voice settings updated successfully.");
}
```

## Speech-to-Text

Convert audio to text:

```csharp
public async Task SpeechToTextExample(IElevenLabsClient client)
{
    // Load audio file
    using var audioStream = File.OpenRead("input_audio.mp3");
    
    // Convert speech to text
    var result = await client.SpeechToText.SpeechToTextAsync(audioStream);
    
    Console.WriteLine($"Transcribed Text: {result.Text}");
    Console.WriteLine($"Language: {result.Language}");
    Console.WriteLine($"Confidence: {result.Confidence}");
}
```

## Voice Changer

Change the voice in an audio file:

```csharp
public async Task VoiceChangerExample(IElevenLabsClient client)
{
    // Load audio file
    using var audioStream = File.OpenRead("input_audio.mp3");
    
    // Get available voices
    var voices = await client.Voices.GetVoicesAsync();
    var targetVoiceId = voices[0].Id;
    
    // Change voice
    using var changedAudioStream = await client.VoiceChanger.ChangeVoiceAsync(
        audioStream, 
        targetVoiceId, 
        modelId: "eleven_monolingual_v1" // Optional: specify model ID
    );
    
    // Save the audio with changed voice
    using var fileStream = File.Create("voice_changed.mp3");
    await changedAudioStream.CopyToAsync(fileStream);
}
```

## Audio Isolation

Isolate voice from background noise in an audio file:

```csharp
public async Task AudioIsolationExample(IElevenLabsClient client)
{
    // Load audio file
    using var audioStream = File.OpenRead("noisy_audio.mp3");
    
    // Isolate voice
    using var isolatedAudioStream = await client.AudioIsolation.IsolateVoiceAsync(audioStream);
    
    // Save the isolated audio
    using var fileStream = File.Create("isolated_voice.mp3");
    await isolatedAudioStream.CopyToAsync(fileStream);
}
```

## Error Handling

The SDK uses custom exceptions to provide detailed error information:

```csharp
try
{
    var voices = await client.Voices.GetVoicesAsync();
}
catch (ElevenLabsAuthenticationException ex)
{
    Console.WriteLine($"Authentication error: {ex.Message}");
    // Handle authentication issues (e.g., invalid API key)
}
catch (ElevenLabsResourceNotFoundException ex)
{
    Console.WriteLine($"Resource not found: {ex.Message}");
    // Handle not found errors
}
catch (ElevenLabsValidationException ex)
{
    Console.WriteLine($"Validation error: {ex.Message}");
    // Handle validation errors
}
catch (ElevenLabsRateLimitExceededException ex)
{
    Console.WriteLine($"Rate limit exceeded: {ex.Message}");
    Console.WriteLine($"Reset time: {ex.ResetTime}");
    // Handle rate limiting
}
catch (ElevenLabsServerException ex)
{
    Console.WriteLine($"Server error: {ex.Message}");
    // Handle server errors
}
catch (ElevenLabsException ex)
{
    Console.WriteLine($"ElevenLabs API error: {ex.Message}");
    Console.WriteLine($"Status code: {ex.StatusCode}");
    Console.WriteLine($"Error code: {ex.ErrorCode}");
    // Handle other API errors
}
```

## Advanced Configuration

### Custom HTTP Client

You can customize the HTTP client used by the SDK:

```csharp
services.AddHttpClient<IElevenLabsHttpClient, ElevenLabsHttpClient>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "MyApplication/1.0");
    // Add any other custom HTTP client configuration
});

services.AddElevenLabs(options =>
{
    options.ApiKey = "your-api-key-here";
});
```

### Dependency Injection in ASP.NET Core

In an ASP.NET Core application, add the SDK to the services in `Program.cs` or `Startup.cs`:

```csharp
builder.Services.AddElevenLabs(options =>
{
    options.ApiKey = builder.Configuration["ElevenLabs:ApiKey"];
    options.TimeoutSeconds = 60;
});
```

Then inject the client into your controllers or services:

```csharp
public class SpeechController : ControllerBase
{
    private readonly IElevenLabsClient _elevenLabsClient;

    public SpeechController(IElevenLabsClient elevenLabsClient)
    {
        _elevenLabsClient = elevenLabsClient;
    }

    [HttpPost("convert")]
    public async Task<IActionResult> ConvertTextToSpeech([FromBody] TextToSpeechModel model)
    {
        try
        {
            var request = new TextToSpeechRequest
            {
                Text = model.Text,
                ModelId = model.ModelId,
                VoiceSettings = new VoiceSettings
                {
                    Stability = model.Stability,
                    SimilarityBoost = model.SimilarityBoost
                }
            };

            using var audioStream = await _elevenLabsClient.TextToSpeech.TextToSpeechAsync(
                model.VoiceId, 
                request
            );

            return File(audioStream, "audio/mpeg", "speech.mp3");
        }
        catch (ElevenLabsException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
```

## API Reference

The SDK provides the following main components:

### IElevenLabsClient

The main entry point for the SDK, providing access to all services:

- `TextToSpeech`: Text-to-speech conversion services
- `Voices`: Voice management services
- `Models`: Model management services
- `SpeechToText`: Speech-to-text conversion services
- `VoiceChanger`: Voice changing services
- `AudioIsolation`: Audio isolation services

### Services

#### ITextToSpeechService

- `TextToSpeechAsync`: Converts text to speech
- `StreamTextToSpeechAsync`: Streams text-to-speech conversion

#### IVoiceService

- `GetVoicesAsync`: Gets all available voices
- `GetVoiceAsync`: Gets a specific voice
- `GetDefaultVoiceSettingsAsync`: Gets default voice settings
- `GetVoiceSettingsAsync`: Gets settings for a specific voice
- `EditVoiceSettingsAsync`: Edits settings for a specific voice
- `DeleteVoiceAsync`: Deletes a voice

#### IModelService

- `GetModelsAsync`: Gets all available models

#### ISpeechToTextService

- `SpeechToTextAsync`: Converts speech to text

#### IVoiceChangerService

- `ChangeVoiceAsync`: Changes the voice in an audio file
- `StreamChangeVoiceAsync`: Streams voice changing process

#### IAudioIsolationService

- `IsolateVoiceAsync`: Isolates voice from an audio file
- `StreamIsolateVoiceAsync`: Streams voice isolation process

## License

This SDK is licensed under the MIT License. See the LICENSE file for details.
