using DustInTheWind.SaltBank.ToolKit.Tests.Helpers;

namespace DustInTheWind.SaltBank.ToolKit.Tests.StatementsDocumentLoadTests;

public class Load_CsvTests
{
    [Fact]
    public void WhenCsvHasSingleRow_ShouldParseAllFields()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        StatementsDocument result = StatementsDocument.Load(csv);

        result.Should().ContainSingle();

        BankTransaction transaction = result[0];
        transaction.Date.Should().Be(new DateOnly(2026, 2, 1));
        transaction.CounterParty.Should().Be("Shop A");
        transaction.Reference.Should().Be("Ref-001");
        transaction.Type.Should().Be("Card");
        transaction.Amount.Should().BeApproximately(-15.75, 1e-10);
        transaction.Balance.Should().BeApproximately(1200.25, 1e-10);
        transaction.SpendingCategory.Should().Be("Groceries");
        transaction.Notes.Should().Be("Weekly shopping");
    }

    [Fact]
    public void WhenCsvHasMultipleRows_ShouldPreserveOrder()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        StatementsDocument result = StatementsDocument.Load(csv);

        result.Should().HaveCount(2);
        result[0].Reference.Should().Be("Ref-001");
        result[1].Reference.Should().Be("Ref-002");
    }

    [Fact]
    public void WhenHeadersContainWhitespace_ShouldTrimAndParse()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        StatementsDocument result = StatementsDocument.Load(csv);

        result.Should().ContainSingle();
        result[0].CounterParty.Should().Be("Store B");
        result[0].Amount.Should().BeApproximately(-30.5, 1e-10);
    }

    [Fact]
    public void WhenCsvContainsBlankLines_ShouldIgnoreBlankLines()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        StatementsDocument result = StatementsDocument.Load(csv);

        result.Should().HaveCount(2);
    }

    [Fact]
    public void WhenNotesColumnIsMissing_ShouldUseDefaultNotesValue()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        StatementsDocument result = StatementsDocument.Load(csv);

        result.Should().ContainSingle();
        result[0].Notes.Should().BeEmpty();
    }

    [Fact]
    public void WhenCsvIsNull_ShouldThrowArgumentNullException()
    {
        Action action = () => StatementsDocument.Load((string)null!);
        action.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void WhenCsvIsEmptyOrWhitespace_ShouldThrow(string csv)
    {
        Action action = () => StatementsDocument.Load(csv);
        action.Should().Throw<StatementDocumentException>();
    }

    [Fact]
    public void WhenRequiredHeaderIsMissing_ShouldThrow()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        Action action = () => StatementsDocument.Load(csv);
        action.Should().Throw<StatementHeaderException>();
    }

    [Fact]
    public void WhenDateFormatIsInvalid_ShouldThrow()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        Action action = () => StatementsDocument.Load(csv);
        action.Should().Throw<StatementDataException>();
    }

    [Fact]
    public void WhenNumericValueIsInvalid_ShouldThrow()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        Action action = () => StatementsDocument.Load(csv);
        action.Should().Throw<StatementDataException>();
    }
}