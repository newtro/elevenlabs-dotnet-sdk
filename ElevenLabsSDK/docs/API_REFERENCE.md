# ElevenLabs SDK API Reference

This document provides detailed information about the classes and interfaces in the ElevenLabs SDK.

## Core Components

### IElevenLabsClient

The main entry point for the SDK, providing access to all services.

```csharp
public interface IElevenLabsClient
{
    ITextToSpeechService TextToSpeech { get; }
    IVoiceService Voices { get; }
    IModelService Models { get; }
    ISpeechToTextService SpeechToText { get; }
    IVoiceChangerService VoiceChanger { get; }
    IAudioIsolationService AudioIsolation { get; }
}
```

### ElevenLabsOptions

Configuration options for the ElevenLabs SDK.

```csharp
public class ElevenLabsOptions
{
    public string ApiKey { get; set; }
    public string BaseUrl { get; set; } = "https://api.elevenlabs.io/v1/";
    public int TimeoutSeconds { get; set; } = 30;
    
    public void Validate();
}
```

## Services

### ITextToSpeechService

Service for text-to-speech conversion.

```csharp
public interface ITextToSpeechService
{
    Task<Stream> TextToSpeechAsync(
        string voiceId, 
        TextToSpeechRequest request, 
        string outputFormat = "mp3_44100_128", 
        CancellationToken cancellationToken = default);
        
    Task<Stream> StreamTextToSpeechAsync(
        string voiceId, 
        TextToSpeechRequest request, 
        string outputFormat = "mp3_44100_128", 
        CancellationToken cancellationToken = default);
}
```

### IVoiceService

Service for managing voices.

```csharp
public interface IVoiceService
{
    Task<List<Voice>> GetVoicesAsync(
        bool showLegacy = false, 
        CancellationToken cancellationToken = default);
        
    Task<Voice> GetVoiceAsync(
        string voiceId, 
        CancellationToken cancellationToken = default);
        
    Task<VoiceSettings> GetDefaultVoiceSettingsAsync(
        CancellationToken cancellationToken = default);
        
    Task<VoiceSettings> GetVoiceSettingsAsync(
        string voiceId, 
        CancellationToken cancellationToken = default);
        
    Task EditVoiceSettingsAsync(
        string voiceId, 
        VoiceSettings settings, 
        CancellationToken cancellationToken = default);
        
    Task DeleteVoiceAsync(
        string voiceId, 
        CancellationToken cancellationToken = default);
}
```

### IModelService

Service for managing models.

```csharp
public interface IModelService
{
    Task<List<Model>> GetModelsAsync(
        CancellationToken cancellationToken = default);
}
```

### ISpeechToTextService

Service for speech-to-text conversion.

```csharp
public interface ISpeechToTextService
{
    Task<SpeechToTextResponse> SpeechToTextAsync(
        Stream audioStream, 
        string modelId = null, 
        CancellationToken cancellationToken = default);
}
```

### IVoiceChangerService

Service for changing voices in audio files.

```csharp
public interface IVoiceChangerService
{
    Task<Stream> ChangeVoiceAsync(
        Stream audioStream, 
        string voiceId, 
        string modelId = null, 
        CancellationToken cancellationToken = default);
        
    Task<Stream> StreamChangeVoiceAsync(
        Stream audioStream, 
        string voiceId, 
        string modelId = null, 
        CancellationToken cancellationToken = default);
}
```

### IAudioIsolationService

Service for isolating voices from audio files.

```csharp
public interface IAudioIsolationService
{
    Task<Stream> IsolateVoiceAsync(
        Stream audioStream, 
        CancellationToken cancellationToken = default);
        
    Task<Stream> StreamIsolateVoiceAsync(
        Stream audioStream, 
        CancellationToken cancellationToken = default);
}
```

## Models

### TextToSpeechRequest

Request model for text-to-speech conversion.

```csharp
public class TextToSpeechRequest
{
    public string Text { get; set; }
    public string ModelId { get; set; }
    public VoiceSettings VoiceSettings { get; set; }
    public List<PronunciationDictionaryLocator> PronunciationDictionaryLocators { get; set; }
}
```

### Voice

Model representing a voice.

```csharp
public class Voice
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public string PreviewUrl { get; set; }
    public bool IsCloned { get; set; }
    public List<string> SamplesAudioUrls { get; set; }
    public VoiceSettings DefaultVoiceSettings { get; set; }
}
```

### VoiceSettings

Model representing voice settings.

```csharp
public class VoiceSettings
{
    public double Stability { get; set; }
    public double SimilarityBoost { get; set; }
    public double Style { get; set; }
    public bool UseSpeakerBoost { get; set; }
    
    public void Validate();
}
```

### Model

Model representing a text-to-speech model.

```csharp
public class Model
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Languages { get; set; }
    public string Type { get; set; }
}
```

### SpeechToTextResponse

Response model for speech-to-text conversion.

```csharp
public class SpeechToTextResponse
{
    public string Text { get; set; }
    public string Language { get; set; }
    public double Confidence { get; set; }
}
```

### PronunciationDictionaryLocator

Model for locating pronunciation dictionaries.

```csharp
public class PronunciationDictionaryLocator
{
    public string Id { get; set; }
    public string Version { get; set; }
}
```

## Exceptions

### ElevenLabsException

Base exception for all ElevenLabs API exceptions.

```csharp
public class ElevenLabsException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string ErrorCode { get; }
    
    public ElevenLabsException(
        string message, 
        HttpStatusCode statusCode, 
        string errorCode = null, 
        Exception innerException = null);
}
```

### ElevenLabsAuthenticationException

Exception thrown when authentication fails.

```csharp
public class ElevenLabsAuthenticationException : ElevenLabsException
{
    public ElevenLabsAuthenticationException(
        string message, 
        HttpStatusCode statusCode = HttpStatusCode.Unauthorized, 
        string errorCode = null, 
        Exception innerException = null);
}
```

### ElevenLabsResourceNotFoundException

Exception thrown when a resource is not found.

```csharp
public class ElevenLabsResourceNotFoundException : ElevenLabsException
{
    public ElevenLabsResourceNotFoundException(
        string message, 
        HttpStatusCode statusCode = HttpStatusCode.NotFound, 
        string errorCode = null, 
        Exception innerException = null);
}
```

### ElevenLabsValidationException

Exception thrown when a request is invalid.

```csharp
public class ElevenLabsValidationException : ElevenLabsException
{
    public object ValidationErrors { get; }
    
    public ElevenLabsValidationException(
        string message, 
        object validationErrors, 
        HttpStatusCode statusCode = HttpStatusCode.BadRequest, 
        string errorCode = null, 
        Exception innerException = null);
}
```

### ElevenLabsRateLimitExceededException

Exception thrown when the API rate limit is exceeded.

```csharp
public class ElevenLabsRateLimitExceededException : ElevenLabsException
{
    public DateTimeOffset? ResetTime { get; }
    
    public ElevenLabsRateLimitExceededException(
        string message, 
        DateTimeOffset? resetTime = null, 
        HttpStatusCode statusCode = HttpStatusCode.TooManyRequests, 
        string errorCode = null, 
        Exception innerException = null);
}
```

### ElevenLabsServerException

Exception thrown when the API returns a server error.

```csharp
public class ElevenLabsServerException : ElevenLabsException
{
    public ElevenLabsServerException(
        string message, 
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError, 
        string errorCode = null, 
        Exception innerException = null);
}
```

## Extension Methods

### ServiceCollectionExtensions

Extension methods for setting up ElevenLabs services in an `IServiceCollection`.

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddElevenLabs(
        this IServiceCollection services, 
        Action<ElevenLabsOptions> configureOptions);
}
```
