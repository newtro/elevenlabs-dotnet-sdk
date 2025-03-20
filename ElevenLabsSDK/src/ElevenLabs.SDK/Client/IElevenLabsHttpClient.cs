using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ElevenLabs.SDK.Client
{
    /// <summary>
    /// Interface for the ElevenLabs HTTP client.
    /// </summary>
    public interface IElevenLabsHttpClient
    {
        /// <summary>
        /// Sends an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>The HTTP response message.</returns>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a GET request to the specified URI as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>The HTTP response message.</returns>
        Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a POST request to the specified URI as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>The HTTP response message.</returns>
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a DELETE request to the specified URI as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The URI the request is sent to.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>The HTTP response message.</returns>
        Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken = default);
    }
}
