# ElevenLabs API Overview

## Introduction
ElevenLabs provides a powerful API for text-to-speech conversion, voice manipulation, and audio processing. The API allows developers to integrate ElevenLabs' state-of-the-art audio models into their applications.

## Authentication
The ElevenLabs API uses API keys for authentication. Every request to the API must include your API key in an `xi-api-key` HTTP header.

API keys can be scoped to:
1. **Scope restriction:** Limit which API endpoints the key can access
2. **Credit quota:** Define custom credit limits to control usage

Example header:
```
xi-api-key: ELEVENLABS_API_KEY
```

## Streaming
The ElevenLabs API supports real-time audio streaming for select endpoints, returning raw audio bytes (e.g., MP3 data) directly over HTTP using chunked transfer encoding. This allows clients to process or play audio incrementally as it is generated.

Streaming is supported for:
- Text to Speech API
- Voice Changer API
- Audio Isolation API

## Main Endpoints

### Text to Speech
- **POST /v1/text-to-speech/:voice_id** - Converts text into speech using a voice of your choice and returns audio
- **POST /v1/text-to-speech/:voice_id/stream** - Streams text-to-speech conversion
- **POST /v1/text-to-speech/:voice_id/with-timing** - Creates speech with timing information
- **STREAM /v1/text-to-speech/:voice_id/stream-with-timing** - Streams speech with timing information

### Voice Management
- **GET /v1/voices** - Lists all available voices
- Additional endpoints for voice creation, editing, and deletion

### Models
- **GET /v1/models** - Lists available models

### History
- Endpoints for managing generation history

## Request Parameters
The Text to Speech endpoint accepts various parameters:
- `text` (Required): The text to convert to speech
- `model_id`: Identifier of the model to use
- `voice_settings`: Voice settings overriding stored settings
- `output_format`: Format of the generated audio (mp3, pcm, etc.)
- Various other parameters for controlling speech generation

## Response Formats
The API can return audio in various formats including:
- MP3 (various sample rates and bitrates)
- PCM
- FLAC
- and others

## Rate Limits and Usage
API usage is tracked and may be subject to rate limits depending on the subscription tier.
