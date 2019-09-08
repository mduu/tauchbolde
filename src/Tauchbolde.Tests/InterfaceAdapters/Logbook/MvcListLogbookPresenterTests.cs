using System;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using Tauchbolde.Application.UseCases.Logbook.ListAllUseCase;
using Tauchbolde.InterfaceAdapters.Logbook.ListAll;
using Tauchbolde.InterfaceAdapters.TextFormatting;
using Xunit;

namespace Tauchbolde.Tests.InterfaceAdapters.Logbook
{
    public class MvcListLogbookPresenterTests
    {
        private readonly ITextFormatter textFormatter = A.Fake<ITextFormatter>();
        private readonly MvcListLogbookOutputPort outputPort;

        public MvcListLogbookPresenterTests()
        {
            A.CallTo(() => textFormatter.GetHtmlText(A<string>._))
                .ReturnsLazily(call => $"{(string)call.Arguments[0]}_formatted");

            outputPort = new MvcListLogbookOutputPort(false, textFormatter);
        }

        [Theory]
        [InlineData("teaser", "text", "teaser_formatted")]
        [InlineData("", "text", "text_formatted")]
        [InlineData(null, "text", "text_formatted")]
        [InlineData("teaser", "", "teaser_formatted")]
        [InlineData("teaser", null, "teaser_formatted")]
        [InlineData("", "", "")]
        [InlineData("", null, null)]
        public void Present_Success(string teaserText, string text, string expectedTeaser)
        {
            // Act
            outputPort.Output(new ListAllLogbookEntriesOutputPort(
                new[]
                {
                    new ListAllLogbookEntriesOutputPort.LogbookItem(
                        new Guid("08D38728-25B6-4778-98E4-BA333ED97206"),
                        "A title",
                        teaserText,
                        "image/1.jpg",
                        true,
                        text)
                }));

            // Assert
            var model = outputPort.GetViewModel();
            model.LogbookItems.Single().TeaserText.Should().Be(expectedTeaser);
        }

        [Fact]
        public void Present_ThrowIfNullInput()
        {
            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            Action act = () => outputPort.Output(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}