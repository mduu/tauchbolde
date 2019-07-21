using System;

namespace Tauchbolde.Application.Services.Core
{
    internal class Clock : IClock
    {
        public DateTimeOffset Now()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}