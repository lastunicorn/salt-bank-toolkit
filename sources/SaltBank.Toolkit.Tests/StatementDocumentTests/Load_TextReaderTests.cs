using DustInTheWind.SaltBank.Toolkit.Tests.Helpers;

namespace DustInTheWind.SaltBank.Toolkit.Tests.StatementDocumentTests;

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