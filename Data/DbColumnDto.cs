namespace MsSQLDatabaseComparison.Data;

public class DbColumnDto
{
	public string TableName { get; set; }
	public string TableSchema { get; set; }
	public string ColumnName { get; set; }
	public string DataType { get; set; }
	public string ColumnDefault { get; set; }
	public string IsNullable { get; set; }
	public int? CharacterMaximumLength { get; set; }
	public byte? NumericPrecision { get; set; }
	public int? NumericScale { get; set; }
}