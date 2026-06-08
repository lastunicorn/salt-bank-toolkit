namespace DustInTheWind.SaltBank.Toolkit;

internal enum DocumentReadState
{
	HeaderRow = 0,
	DataRow,
	Ended
}