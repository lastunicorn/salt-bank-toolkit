using DustInTheWind.SaltBank.Toolkit.Tests.Helpers;

namespace DustInTheWind.SaltBank.Toolkit.Tests.StatementDocumentTests;

public class Load_TextReaderTests
{
	[Fact]
	public void WhenTextReaderThrows_ShouldThrowStatementDocumentException()
	{
		using FailingTextReader failingTextReader = new();

		Action action = async () => StatementDocument.LoadAsync(failingTextReader).GetAwaiter().GetResult();
		action.Should().Throw<DocumentLoadException>()
			.WithInnerException<IOException>();
	}
}