using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;

namespace Tauchbolde.Common.Infrastructure.Telemetry
{
    /// <summary>
    /// <see cref="ITelemetryService"/> implementation using Application Insights.
    /// </summary>
    public class AppInsightsTelemetryService : ITelemetryService
    {
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Tauchbolde.Common.Telemetry.AppInsightsTelemetryService"/> class.
        /// </summary>
        /// <param name="telemetryClient">Telemetry client.</param>
        public AppInsightsTelemetryService(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        /// <inheritdoc/>
        public void TrackEvent(string name, IDictionary<string, string> data = null)
        {
            telemetryClient.TrackEvent(name, data);
        }
    }
}
