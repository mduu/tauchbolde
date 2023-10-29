using JetBrains.Annotations;

namespace Tauchbolde.Application.Services.Telemetry
{
    /// <summary>
    /// Telemetry service interface.
    /// </summary>
    public interface ITelemetryService
    {
        void TrackEvent([NotNull] string name, IDictionary<string, string> data = null);
        void TrackEvent([NotNull] string name, [CanBeNull] object dataObject);
    }
}
