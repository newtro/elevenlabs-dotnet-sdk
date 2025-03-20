# ElevenLabs API Data Models

## Voice Model
```csharp
public class Voice
{
    public string VoiceId { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public Dictionary<string, string> Labels { get; set; }
    public string Description { get; set; }
    public string Gender { get; set; }
    public string UseCase { get; set; }
    public List<string> AvailableForTiers { get; set; }
    public List<string> SupportedModels { get; set; }
    public VoiceSettings Settings { get; set; }
}

public class VoiceSettings
{
    public double Stability { get; set; }
    public double SimilarityBoost { get; set; }
    public double Style { get; set; }
    public bool UseSpeakerBoost { get; set; }
}
```

## Text-to-Speech Models
```csharp
public class TextToSpeechRequest
{
    public string Text { get; set; }
    public string ModelId { get; set; }
    public string LanguageCode { get; set; }
    public VoiceSettings VoiceSettings { get; set; }
    public List<PronunciationDictionaryLocator> PronunciationDictionaryLocators { get; set; }
    public int? Seed { get; set; }
    public string PreviousText { get; set; }
    public string NextText { get; set; }
    public List<string> PreviousRequestIds { get; set; }
    public List<string> NextRequestIds { get; set; }
    public string ApplyTextNormalization { get; set; }
}

public class PronunciationDictionaryLocator
{
    public string Id { get; set; }
    public string VersionId { get; set; }
}

public enum OutputFormat
{
    Mp3_44100_128,
    Mp3_44100_192,
    Pcm_16000,
    Pcm_22050,
    Pcm_24000,
    Pcm_44100,
    Ulaw_8000
}
```

## Model Information
```csharp
public class Model
{
    public string ModelId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> SupportedLanguages { get; set; }
    public bool CanDoTextToSpeech { get; set; }
    public bool CanDoVoiceConversion { get; set; }
    public string TokenCostFactor { get; set; }
}
```

## History Models
```csharp
public class HistoryItem
{
    public string HistoryItemId { get; set; }
    public string RequestId { get; set; }
    public string VoiceId { get; set; }
    public string VoiceName { get; set; }
    public string Text { get; set; }
    public DateTime DateUnix { get; set; }
    public string CharacterCountChangeFrom { get; set; }
    public string CharacterCountChangeTo { get; set; }
    public string ContentType { get; set; }
    public string State { get; set; }
    public HistorySettings Settings { get; set; }
    public string FeedbackId { get; set; }
}

public class HistorySettings
{
    public double Stability { get; set; }
    public double SimilarityBoost { get; set; }
    public double Style { get; set; }
    public bool UseSpeakerBoost { get; set; }
    public string ModelId { get; set; }
}
```

## Error Models
```csharp
public class ElevenLabsError
{
    public string Status { get; set; }
    public string Message { get; set; }
    public Dictionary<string, List<string>> ValidationErrors { get; set; }
}
```

## API Response Models
```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public ElevenLabsError Error { get; set; }
}

public class VoicesResponse
{
    public List<Voice> Voices { get; set; }
}

public class ModelsResponse
{
    public List<Model> Models { get; set; }
}
```
