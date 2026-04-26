using CsvHelper;
using CsvHelper.TypeConversion;
using DustInTheWind.SaltBank.ToolKit.Tests.Helpers;

namespace DustInTheWind.SaltBank.ToolKit.Tests.StatementsDocumentLoadTests;

public class LoadTests
{
    [Fact]
    public void WhenCsvHasSingleRow_ShouldParseAllFields()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        StatementsDocument result = StatementsDocument.Load(csv);

        Assert.Single(result);

        BankTransaction transaction = result[0];
        Assert.Equal(new DateOnly(2026, 2, 1), transaction.Date);
        Assert.Equal("Shop A", transaction.CounterParty);
        Assert.Equal("Ref-001", transaction.Reference);
        Assert.Equal("Card", transaction.Type);
        Assert.Equal(-15.75, transaction.Amount, 10);
        Assert.Equal(1200.25, transaction.Balance, 10);
        Assert.Equal("Groceries", transaction.SpendingCategory);
        Assert.Equal("Weekly shopping", transaction.Notes);
    }

    [Fact]
    public void WhenCsvHasMultipleRows_ShouldPreserveOrder()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        StatementsDocument result = StatementsDocument.Load(csv);

        Assert.Equal(2, result.Count);
        Assert.Equal("Ref-001", result[0].Reference);
        Assert.Equal("Ref-002", result[1].Reference);
    }

    [Fact]
    public void WhenHeadersContainWhitespace_ShouldTrimAndParse()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        StatementsDocument result = StatementsDocument.Load(csv);

        Assert.Single(result);
        Assert.Equal("Store B", result[0].CounterParty);
        Assert.Equal(-30.5, result[0].Amount, 10);
    }

    [Fact]
    public void WhenCsvContainsBlankLines_ShouldIgnoreBlankLines()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        StatementsDocument result = StatementsDocument.Load(csv);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void WhenNotesColumnIsMissing_ShouldUseDefaultNotesValue()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        StatementsDocument result = StatementsDocument.Load(csv);

        Assert.Single(result);
        Assert.Equal(string.Empty, result[0].Notes);
    }

    [Fact]
    public void WhenCsvIsNull_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => StatementsDocument.Load((string)null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void WhenCsvIsEmptyOrWhitespace_ShouldThrowArgumentException(string csv)
    {
        Assert.Throws<ArgumentException>(() => StatementsDocument.Load(csv));
    }

    [Fact]
    public void WhenRequiredHeaderIsMissing_ShouldThrowHeaderValidationException()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        Assert.Throws<HeaderValidationException>(() => StatementsDocument.Load(csv));
    }

    [Fact]
    public void WhenDateFormatIsInvalid_ShouldThrowReaderException()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        ReaderException exception = Assert.Throws<ReaderException>(() => StatementsDocument.Load(csv));
        Assert.IsType<FormatException>(exception.InnerException);
    }

    [Fact]
    public void WhenNumericValueIsInvalid_ShouldThrowTypeConverterException()
    {
        string csv = TestResources.GetEmbeddedResourceAsText(FileExtension.Csv);

        Assert.Throws<TypeConverterException>(() => StatementsDocument.Load(csv));
    }
}