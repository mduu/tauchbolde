﻿using FluentAssertions;
using Tauchbolde.SharedKernel.Extensions;
using Xunit;

namespace Tauchbolde.Tests.SharedKernel.Extensions
{
    public class ExceptionExtensionsTests
    {
        [Fact]
        public void UnwindWithoutInnerException()
        {
            var ex = new Exception("Hi Error.");
            
            var result = ex.UnwindMessage();
            
            result.Should().NotBeNull();
            result.Should().Be("Hi Error.");
        }
        
        [Fact]
        public void UnwindWithInnerException()
        {
            var innerEx = new Exception("Hi Inner Error.");
            var ex = new Exception("Hi Error.", innerEx);
            
            var result = ex.UnwindMessage();
            
            result.Should().NotBeNull();
            result.Should().Be("Hi Error. Hi Inner Error.");
        }
        
        [Fact]
        public void UnwindWithMultipleInnerException()
        {
            var innerMostEx = new Exception("Hi Inner Most Exceptions.");
            var innerEx = new Exception("Hi Inner Error.", innerMostEx);
            var ex = new Exception("Hi Error.", innerEx);
            
            var result = ex.UnwindMessage();
            
            result.Should().NotBeNull();
            result.Should().Be("Hi Error. Hi Inner Error. Hi Inner Most Exceptions.");
        }
    }
}
