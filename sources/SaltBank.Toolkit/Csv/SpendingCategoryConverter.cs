using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace DustInTheWind.SaltBank.Toolkit.Csv;

internal sealed class SpendingCategoryConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new TypeConverterException(this, memberMapData, text, row.Context, "Spending category cannot be empty.");

        try
        {
            return new SpendingCategory(text);
        }
        catch (ArgumentException ex)
        {
            throw new TypeConverterException(this, memberMapData, text, row.Context, ex.Message, ex);
        }
    }

    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        return value is SpendingCategory spendingCategory
            ? spendingCategory.Value
            : base.ConvertToString(value, row, memberMapData);
    }
}