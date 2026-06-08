using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace DustInTheWind.SaltBank.Toolkit.Csv;

internal sealed class TransactionTypeConverter : DefaultTypeConverter
{
	public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
	{
		if (string.IsNullOrWhiteSpace(text))
			throw new TypeConverterException(this, memberMapData, text, row.Context, "Transaction type cannot be empty.");

		try
		{
			return new TransactionType(text);
		}
		catch (ArgumentException ex)
		{
			throw new TypeConverterException(this, memberMapData, text, row.Context, ex.Message, ex);
		}
	}

	public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
	{
		return value is TransactionType transactionType
			? transactionType.Value
			: base.ConvertToString(value, row, memberMapData);
	}
}