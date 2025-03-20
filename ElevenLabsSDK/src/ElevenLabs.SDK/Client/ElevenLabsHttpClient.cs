using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using ElevenLabs.SDK.Configuration;
using ElevenLabs.SDK.Exceptions;
using Microsoft.Extensions.Options;

namespace ElevenLabs.SDK.Client
{
    /// <summary>
    /// Base HTTP client for making requests to the ElevenLabs API.
    /// </summary>
    public class ElevenLabsHttpClient : IElevenLabsHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ElevenLabsOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElevenLabsHttpClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="options">The ElevenLabs API options.</param>
        public ElevenLabsHttpClient(HttpClient httpClient, IOptions<ElevenLabsOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            
            _options.Validate();
            
            // Configure the HttpClient
            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("xi-api-key", _options.ApiKey);
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
                await ResponseHandler.EnsureSuccessStatusCodeAsync(response).ConfigureAwait(false);
                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new ElevenLabsException("An error occurred while sending the request.", System.Net.HttpStatusCode.ServiceUnavailable, null, ex);
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                throw new ElevenLabsException("The request timed out.", System.Net.HttpStatusCode.RequestTimeout, null, ex);
            }
            catch (TaskCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException("The request was canceled.", ex);
            }
            catch (Exception ex) when (!(ex is ElevenLabsException))
            {
                throw new ElevenLabsException("An unexpected error occurred.", System.Net.HttpStatusCode.InternalServerError, null, ex);
            }
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
                await ResponseHandler.EnsureSuccessStatusCodeAsync(response).ConfigureAwait(false);
                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new ElevenLabsException("An error occurred while sending the GET request.", System.Net.HttpStatusCode.ServiceUnavailable, null, ex);
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                throw new ElevenLabsException("The GET request timed out.", System.Net.HttpStatusCode.RequestTimeout, null, ex);
            }
            catch (TaskCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException("The GET request was canceled.", ex);
            }
            catch (Exception ex) when (!(ex is ElevenLabsException))
            {
                throw new ElevenLabsException("An unexpected error occurred during the GET request.", System.Net.HttpStatusCode.InternalServerError, null, ex);
            }
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);
                await ResponseHandler.EnsureSuccessStatusCodeAsync(response).ConfigureAwait(false);
                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new ElevenLabsException("An error occurred while sending the POST request.", System.Net.HttpStatusCode.ServiceUnavailable, null, ex);
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                throw new ElevenLabsException("The POST request timed out.", System.Net.HttpStatusCode.RequestTimeout, null, ex);
            }
            catch (TaskCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException("The POST request was canceled.", ex);
            }
            catch (Exception ex) when (!(ex is ElevenLabsException))
            {
                throw new ElevenLabsException("An unexpected error occurred during the POST request.", System.Net.HttpStatusCode.InternalServerError, null, ex);
            }
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(requestUri, cancellationToken).ConfigureAwait(false);
                await ResponseHandler.EnsureSuccessStatusCodeAsync(response).ConfigureAwait(false);
                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new ElevenLabsException("An error occurred while sending the DELETE request.", System.Net.HttpStatusCode.ServiceUnavailable, null, ex);
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                throw new ElevenLabsException("The DELETE request timed out.", System.Net.HttpStatusCode.RequestTimeout, null, ex);
            }
            catch (TaskCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException("The DELETE request was canceled.", ex);
            }
            catch (Exception ex) when (!(ex is ElevenLabsException))
            {
                throw new ElevenLabsException("An unexpected error occurred during the DELETE request.", System.Net.HttpStatusCode.InternalServerError, null, ex);
            }
        }
    }
}
