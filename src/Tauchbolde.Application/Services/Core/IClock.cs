using System;

namespace Tauchbolde.Application.Services.Core
{
    public interface IClock
    {
        DateTimeOffset Now();
    }
}