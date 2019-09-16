using System;

namespace Tauchbolde.Application.Services.Core
{
    internal class Clock : IClock
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
    }        
}