namespace MsSQLDatabaseComparison.Data;

public class DbColumnNumericDto
{
	public string TableName { get; set; }
	public string TableSchema { get; set; }
	public string ColumnName { get; set; }
	public string SourceDataType { get; set; }
	public byte? SourceNumericPrecision { get; set; }
	public int? SourceNumericScale { get; set; }
	public string TargetDataType { get; set; }
	public byte? TargetNumericPrecision { get; set; }
	public int? TargetNumericScale { get; set; }
}