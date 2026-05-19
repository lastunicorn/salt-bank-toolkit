using DustInTheWind.SaltBank.ToolKit.Tests.Helpers;

namespace DustInTheWind.SaltBank.ToolKit.Tests.StatementDocumentTests;

public class Load_TextReaderTests
{
    [Fact]
    public void WhenTextReaderThrows_ShouldThrowStatementDocumentException()
    {
        using FailingTextReader failingTextReader = new();

        Action action = () => StatementDocument.Load(failingTextReader);
        action.Should().Throw<StatementDocumentException>()
            .WithInnerException<IOException>();
    }
}