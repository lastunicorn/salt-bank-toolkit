using CsvHelper;
using CsvHelper.TypeConversion;

namespace DustInTheWind.SaltBank.ToolKit.Tests.StatementsDocumentLoadTests;

public class LoadTests
{
    [Fact]
    public void Load_WithSingleRow_ParsesAllFields()
    {
        string csv =
            "Date,Counter Party,Reference,Type,Amount (RON),Balance (RON),Spending Category,Notes\n" +
            "01/02/2026,Shop A,Ref-001,Card,-15.75,1200.25,Groceries,Weekly shopping";

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
    public void Load_WithMultipleRows_PreservesOrder()
    {
        string csv =
            "Date,Counter Party,Reference,Type,Amount (RON),Balance (RON),Spending Category,Notes\n" +
            "01/02/2026,Shop A,Ref-001,Card,-15.75,1200.25,Groceries,Weekly shopping\n" +
            "02/02/2026,Employer,Ref-002,Transfer,3000,4200.25,Salary,Monthly salary";

        StatementsDocument result = StatementsDocument.Load(csv);

        Assert.Equal(2, result.Count);
        Assert.Equal("Ref-001", result[0].Reference);
        Assert.Equal("Ref-002", result[1].Reference);
    }

    [Fact]
    public void Load_WithHeaderWhitespace_TrimsHeadersAndParses()
    {
        string csv =
            " Date , Counter Party , Reference , Type , Amount (RON) , Balance (RON) , Spending Category , Notes \n" +
            "03/02/2026,Store B,Ref-003,Card,-30.5,1169.75,Shopping,New shoes";

        StatementsDocument result = StatementsDocument.Load(csv);

        Assert.Single(result);
        Assert.Equal("Store B", result[0].CounterParty);
        Assert.Equal(-30.5, result[0].Amount, 10);
    }

    [Fact]
    public void Load_WithBlankLines_IgnoresBlankLines()
    {
        string csv =
            "Date,Counter Party,Reference,Type,Amount (RON),Balance (RON),Spending Category,Notes\n\n" +
            "04/02/2026,Store C,Ref-004,Card,-12,1157.75,Food,Lunch\n\n" +
            "05/02/2026,Store D,Ref-005,Card,-8,1149.75,Food,Coffee\n";

        StatementsDocument result = StatementsDocument.Load(csv);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Load_WithoutNotesColumn_UsesDefaultNotesValue()
    {
        string csv =
            "Date,Counter Party,Reference,Type,Amount (RON),Balance (RON),Spending Category\n" +
            "06/02/2026,Store E,Ref-006,Card,-20,1129.75,Bills";

        StatementsDocument result = StatementsDocument.Load(csv);

        Assert.Single(result);
        Assert.Equal(string.Empty, result[0].Notes);
    }

    [Fact]
    public void Load_WithNullCsv_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => StatementsDocument.Load((string)null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Load_WithEmptyOrWhitespaceCsv_ThrowsArgumentException(string csv)
    {
        Assert.Throws<ArgumentException>(() => StatementsDocument.Load(csv));
    }

    [Fact]
    public void Load_WithoutRequiredHeader_ThrowsHeaderValidationException()
    {
        string csv =
            "Date,Counter Party,Reference,Type,Amount (RON),Balance (RON),Notes\n" +
            "07/02/2026,Store F,Ref-007,Card,-10,1119.75,Missing category";

        Assert.Throws<HeaderValidationException>(() => StatementsDocument.Load(csv));
    }

    [Fact]
    public void Load_WithInvalidDate_ThrowsReaderException()
    {
        string csv =
            "Date,Counter Party,Reference,Type,Amount (RON),Balance (RON),Spending Category,Notes\n" +
            "2026-02-08,Store G,Ref-008,Card,-10,1109.75,Food,Invalid date format";

        ReaderException exception = Assert.Throws<ReaderException>(() => StatementsDocument.Load(csv));
        Assert.IsType<FormatException>(exception.InnerException);
    }

    [Fact]
    public void Load_WithInvalidNumericValue_ThrowsTypeConverterException()
    {
        string csv =
            "Date,Counter Party,Reference,Type,Amount (RON),Balance (RON),Spending Category,Notes\n" +
            "08/02/2026,Store H,Ref-009,Card,not-a-number,1099.75,Food,Invalid amount";

        Assert.Throws<TypeConverterException>(() => StatementsDocument.Load(csv));
    }
}