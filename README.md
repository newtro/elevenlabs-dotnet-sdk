# Newtro ElevenLabs .NET SDK

[![NuGet](https://img.shields.io/nuget/v/Newtro.ElevenLabs.DotNet.SDK.svg)](https://www.nuget.org/packages/Newtro.ElevenLabs.DotNet.SDK/)

A comprehensive .NET 8 SDK for integrating with the ElevenLabs API. This SDK provides access to text-to-speech, voice management, speech-to-text, voice changing, and audio isolation features.

## Installation

Install the package via NuGet:

```bash
dotnet add package Newtro.ElevenLabs.DotNet.SDK
```

Or via the Package Manager Console:

```powershell
Install-Package Newtro.ElevenLabs.DotNet.SDK
```

## Getting Started

### Basic Setup

```csharp
// Add services to the DI container
services.AddElevenLabs(options =>
{
    options.ApiKey = "your-api-key-here";
});

// Or create a client directly
var client = new ElevenLabsClient("your-api-key-here");
```

## Examples

### Text-to-Speech

```csharp
// Get a list of available voices
var voices = await client.Voices.GetVoicesAsync();

// Convert text to speech
var request = new TextToSpeechRequest
{
    Text = "Hello, world! This is ElevenLabs text-to-speech API.",
    ModelId = "eleven_monolingual_v1",
    VoiceSettings = new VoiceSettings
    {
        Stability = 0.5f,
        SimilarityBoost = 0.75f
    }
};

// Get the audio as a stream
using var audioStream = await client.TextToSpeech.TextToSpeechAsync(
    "21m00Tcm4TlvDq8ikWAM", // Voice ID
    request
);

// Save to file
using var fileStream = File.Create("output.mp3");
await audioStream.CopyToAsync(fileStream);
```

### Voice Management

```csharp
// Get all available voices
var voices = await client.Voices.GetVoicesAsync();

// Get a specific voice
var voice = await client.Voices.GetVoiceAsync("21m00Tcm4TlvDq8ikWAM");

// Get voice settings
var settings = await client.Voices.GetVoiceSettingsAsync("21m00Tcm4TlvDq8ikWAM");

// Edit voice settings
await client.Voices.EditVoiceSettingsAsync("21m00Tcm4TlvDq8ikWAM", new VoiceSettings
{
    Stability = 0.7f,
    SimilarityBoost = 0.8f
});
```

### Speech-to-Text

```csharp
// Convert speech to text
using var fileStream = File.OpenRead("speech.mp3");
var text = await client.SpeechToText.SpeechToTextAsync(fileStream);
Console.WriteLine(text);
```

### Voice Changer

```csharp
// Change voice in an audio file
using var inputStream = File.OpenRead("input.mp3");
using var outputStream = await client.VoiceChanger.ChangeVoiceAsync(
    inputStream,
    "21m00Tcm4TlvDq8ikWAM", // Target voice ID
    new VoiceSettings
    {
        Stability = 0.5f,
        SimilarityBoost = 0.75f
    }
);

// Save to file
using var fileStream = File.Create("changed_voice.mp3");
await outputStream.CopyToAsync(fileStream);
```

### Audio Isolation

```csharp
// Isolate voice from an audio file
using var inputStream = File.OpenRead("mixed_audio.mp3");
using var outputStream = await client.AudioIsolation.IsolateVoiceAsync(inputStream);

// Save to file
using var fileStream = File.Create("isolated_voice.mp3");
await outputStream.CopyToAsync(fileStream);
```

## Error Handling

The SDK uses custom exceptions for error handling:

```csharp
try
{
    var voices = await client.Voices.GetVoicesAsync();
}
catch (ElevenLabsAuthenticationException ex)
{
    // Handle authentication errors
    Console.WriteLine($"Authentication failed: {ex.Message}");
}
catch (ElevenLabsRateLimitException ex)
{
    // Handle rate limit errors
    Console.WriteLine($"Rate limit exceeded: {ex.Message}");
}
catch (ElevenLabsException ex)
{
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
