using System;

namespace Tauchbolde.SharedKernel.Services
{
    public static class SystemClock
    {
        private static DateTime? _staticTime;

        public static DateTime Now => _staticTime ?? DateTime.UtcNow;

        /// <summary>
        /// This method is used by unit-tests to force a specific time.
        /// </summary>
        /// <param name="currentTime">The current time to return on calls to <see cref="Now"/>.</param>
        internal static void SetTime(DateTime currentTime)
            => _staticTime = currentTime;
    }
}