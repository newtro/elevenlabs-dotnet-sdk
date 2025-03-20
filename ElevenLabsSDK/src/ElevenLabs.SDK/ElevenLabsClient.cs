using System;
using System.Collections.Generic;
using ElevenLabs.SDK.Client;
using ElevenLabs.SDK.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ElevenLabs.SDK.Configuration;

namespace ElevenLabs.SDK
{
    /// <summary>
    /// The main entry point for the ElevenLabs SDK.
    /// </summary>
    public class ElevenLabsClient : IElevenLabsClient
    {
        /// <summary>
        /// Gets the text-to-speech service.
        /// </summary>
        public ITextToSpeechService TextToSpeech { get; }

        /// <summary>
        /// Gets the voice service.
        /// </summary>
        public IVoiceService Voices { get; }

        /// <summary>
        /// Gets the model service.
        /// </summary>
        public IModelService Models { get; }

        /// <summary>
        /// Gets the speech-to-text service.
        /// </summary>
        public ISpeechToTextService SpeechToText { get; }

        /// <summary>
        /// Gets the voice changer service.
        /// </summary>
        public IVoiceChangerService VoiceChanger { get; }

        /// <summary>
        /// Gets the audio isolation service.
        /// </summary>
        public IAudioIsolationService AudioIsolation { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevenLabsClient"/> class.
        /// </summary>
        /// <param name="textToSpeechService">The text-to-speech service.</param>
        /// <param name="voiceService">The voice service.</param>
        /// <param name="modelService">The model service.</param>
        /// <param name="speechToTextService">The speech-to-text service.</param>
        /// <param name="voiceChangerService">The voice changer service.</param>
        /// <param name="audioIsolationService">The audio isolation service.</param>
        public ElevenLabsClient(
            ITextToSpeechService textToSpeechService,
            IVoiceService voiceService,
            IModelService modelService,
            ISpeechToTextService speechToTextService,
            IVoiceChangerService voiceChangerService,
            IAudioIsolationService audioIsolationService)
        {
            TextToSpeech = textToSpeechService ?? throw new ArgumentNullException(nameof(textToSpeechService));
            Voices = voiceService ?? throw new ArgumentNullException(nameof(voiceService));
            Models = modelService ?? throw new ArgumentNullException(nameof(modelService));
            SpeechToText = speechToTextService ?? throw new ArgumentNullException(nameof(speechToTextService));
            VoiceChanger = voiceChangerService ?? throw new ArgumentNullException(nameof(voiceChangerService));
            AudioIsolation = audioIsolationService ?? throw new ArgumentNullException(nameof(audioIsolationService));
        }
    }

    /// <summary>
    /// Interface for the ElevenLabs client.
    /// </summary>
    public interface IElevenLabsClient
    {
        /// <summary>
        /// Gets the text-to-speech service.
        /// </summary>
        ITextToSpeechService TextToSpeech { get; }

        /// <summary>
        /// Gets the voice service.
        /// </summary>
        IVoiceService Voices { get; }

        /// <summary>
        /// Gets the model service.
        /// </summary>
        IModelService Models { get; }

        /// <summary>
        /// Gets the speech-to-text service.
        /// </summary>
        ISpeechToTextService SpeechToText { get; }

        /// <summary>
        /// Gets the voice changer service.
        /// </summary>
        IVoiceChangerService VoiceChanger { get; }

        /// <summary>
        /// Gets the audio isolation service.
        /// </summary>
        IAudioIsolationService AudioIsolation { get; }
    }

    /// <summary>
    /// Extension methods for setting up ElevenLabs services in an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds ElevenLabs services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="configureOptions">A delegate to configure the <see cref="ElevenLabsOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddElevenLabs(this IServiceCollection services, Action<ElevenLabsOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.Configure(configureOptions);
            services.AddElevenLabsCore();

            return services;
        }

        /// <summary>
        /// Adds ElevenLabs services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        private static IServiceCollection AddElevenLabsCore(this IServiceCollection services)
        {
            services.AddHttpClient<IElevenLabsHttpClient, ElevenLabsHttpClient>();
            services.TryAddSingleton<IElevenLabsClient, ElevenLabsClient>();
            services.TryAddSingleton<ITextToSpeechService, TextToSpeechService>();
            services.TryAddSingleton<IVoiceService, VoiceService>();
            services.TryAddSingleton<IModelService, ModelService>();
            services.TryAddSingleton<ISpeechToTextService, SpeechToTextService>();
            services.TryAddSingleton<IVoiceChangerService, VoiceChangerService>();
            services.TryAddSingleton<IAudioIsolationService, AudioIsolationService>();

            return services;
        }
    }
}
