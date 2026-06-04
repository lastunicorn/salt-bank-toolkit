using DustInTheWind.SaltBank.Toolkit.Tests.Helpers;

namespace DustInTheWind.SaltBank.Toolkit.Tests.StatementDocumentTests;

public class Load_CsvTests
{
    [Fact]
    public void WhenCsvHasSingleRow_ShouldParseAllFields()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        StatementDocument result = StatementDocument.LoadAsync(csv).GetAwaiter().GetResult();

        result.Should().ContainSingle();

        BankTransaction transaction = result[0];
        transaction.Date.Should().Be(new DateOnly(2026, 2, 1));
        transaction.CounterParty.Should().Be("Shop A");
        transaction.Reference.Should().Be("Ref-001");
        transaction.Type.Value.Should().Be("Card Payment");
        transaction.Amount.Should().Be(-15.75m);
        transaction.Balance.Should().Be(1200.25m);
        transaction.SpendingCategory.Value.Should().Be("Groceries");
        transaction.Notes.Should().Be("Weekly shopping");
    }

    [Fact]
    public void WhenCsvHasMultipleRows_ShouldPreserveOrder()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        StatementDocument result = StatementDocument.LoadAsync(csv).GetAwaiter().GetResult();

        result.Should().HaveCount(2);
        result[0].Reference.Should().Be("Ref-001");
        result[1].Reference.Should().Be("Ref-002");
    }

    [Fact]
    public void WhenHeadersContainWhitespace_ShouldTrimAndParse()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        StatementDocument result = StatementDocument.LoadAsync(csv).GetAwaiter().GetResult();

        result.Should().ContainSingle();
        result[0].CounterParty.Should().Be("Store B");
        result[0].Amount.Should().Be(-30.5m);
    }

    [Fact]
    public void WhenCsvContainsBlankLines_ShouldIgnoreBlankLines()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        StatementDocument result = StatementDocument.LoadAsync(csv).GetAwaiter().GetResult();

        result.Should().HaveCount(2);
    }

    [Fact]
    public void WhenNotesColumnIsMissing_ShouldUseDefaultNotesValue()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        StatementDocument result = StatementDocument.LoadAsync(csv).GetAwaiter().GetResult();

        result.Should().ContainSingle();
        result[0].Notes.Should().BeNullOrEmpty();
    }

    [Fact]
    public void WhenCsvIsNull_ShouldThrowArgumentException()
    {
        Action action = () => StatementDocument.LoadAsync((string)null!).GetAwaiter().GetResult();
        action.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void WhenCsvIsEmptyOrWhitespace_ShouldThrow(string csv)
    {
        Action action = () => StatementDocument.LoadAsync(csv).GetAwaiter().GetResult();
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void WhenRequiredHeaderIsMissing_ShouldThrow()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        Action action = () => StatementDocument.LoadAsync(csv).GetAwaiter().GetResult();
        action.Should().Throw<DocumentLoadException>();
    }

    [Fact]
    public void WhenDateFormatIsInvalid_ShouldThrow()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        Action action = () => StatementDocument.LoadAsync(csv).GetAwaiter().GetResult();
        action.Should().Throw<DocumentLoadException>();
    }

    [Fact]
    public void WhenNumericValueIsInvalid_ShouldThrow()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        Action action = () => StatementDocument.LoadAsync(csv).GetAwaiter().GetResult();
        action.Should().Throw<DocumentLoadException>();
    }
}