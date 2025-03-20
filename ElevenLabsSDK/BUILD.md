# Build and Package Instructions for ElevenLabs .NET SDK

This document provides instructions for building and packaging the ElevenLabs .NET SDK for distribution.

## Prerequisites

- .NET 8.0 SDK or later
- NuGet CLI (for packaging)

## Building the SDK

### Build in Debug Mode

```bash
cd /path/to/ElevenLabsSDK
dotnet build
```

### Build in Release Mode

```bash
cd /path/to/ElevenLabsSDK
dotnet build -c Release
```

## Running Tests

```bash
cd /path/to/ElevenLabsSDK
dotnet test
```

## Packaging for NuGet

### Using .csproj Settings

```bash
cd /path/to/ElevenLabsSDK/src/ElevenLabs.SDK
dotnet pack -c Release
```

The NuGet package will be created in the `bin/Release` directory.

### Using .nuspec File

```bash
cd /path/to/ElevenLabsSDK
nuget pack ElevenLabs.SDK.nuspec
```

## Publishing to NuGet

### Using dotnet CLI

```bash
dotnet nuget push bin/Release/ElevenLabs.SDK.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

### Using NuGet CLI

```bash
nuget push ElevenLabs.SDK.1.0.0.nupkg -ApiKey YOUR_API_KEY -Source https://api.nuget.org/v3/index.json
```

## Running the Example Project

```bash
cd /path/to/ElevenLabsSDK/examples/ElevenLabs.SDK.Examples
dotnet run
```

Remember to replace the API key in the example project with your actual ElevenLabs API key before running.

## Project Structure

- `src/ElevenLabs.SDK`: Main SDK project
- `tests/ElevenLabs.SDK.Tests`: Unit tests
- `examples/ElevenLabs.SDK.Examples`: Example usage
- `docs`: Documentation files
- `ElevenLabs.SDK.nuspec`: NuGet package specification
- `LICENSE`: MIT license file
