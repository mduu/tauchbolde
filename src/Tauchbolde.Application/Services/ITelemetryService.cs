using System.Collections.Generic;

namespace Tauchbolde.Application.Services
{
    /// <summary>
    /// Telemetry service interface.
    /// </summary>
    public interface ITelemetryService
    {
        /// <summary>
        /// Track a telemetry event.
        /// </summary>
        /// <param name="name">Name of the event to track.</param>
        /// <param name="data">Optional additional data.</param>
        void TrackEvent(string name, IDictionary<string, string> data = null);
    }
}
