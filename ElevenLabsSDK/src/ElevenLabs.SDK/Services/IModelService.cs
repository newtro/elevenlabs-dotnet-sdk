using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ElevenLabs.SDK.Models;

namespace ElevenLabs.SDK.Services
{
    /// <summary>
    /// Interface for the ElevenLabs Model service.
    /// </summary>
    public interface IModelService
    {
        /// <summary>
        /// Gets all available models.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A list of available models.</returns>
        Task<List<Model>> GetModelsAsync(CancellationToken cancellationToken = default);
    }
}
