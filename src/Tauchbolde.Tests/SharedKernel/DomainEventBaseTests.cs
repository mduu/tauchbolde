using System;
using FluentAssertions;
using Tauchbolde.SharedKernel;
using Tauchbolde.SharedKernel.Services;
using Xunit;

namespace Tauchbolde.Tests.SharedKernel
{
    public class DomainEventBaseTests
    {
        [Fact]
        public void Ctor_SetDateOccured()
        {
            // Arrange
            var currentTime = DateTime.UtcNow;
            SystemClock.SetTime(currentTime);
            
            // Act
            var evt = new TestEvent();

            // Assert
            evt.DateOccurred.Should().Be(currentTime);
        }

        private class TestEvent : DomainEventBase
        {
        }
    }
}