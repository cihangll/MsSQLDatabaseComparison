namespace MsSQLDatabaseComparison.Data;

public class DbColumnDataTypeDto
{
	public string TableName { get; set; }
	public string TableSchema { get; set; }
	public string ColumnName { get; set; }
	public string SourceDataType { get; set; }
	public int? SourceCharacterMaximumLength { get; set; }
	public string TargetDataType { get; set; }
	public int? TargetCharacterMaximumLength { get; set; }
}
