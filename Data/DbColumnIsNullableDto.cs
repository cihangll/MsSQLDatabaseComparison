namespace MsSQLDatabaseComparison.Data;

public class DbColumnIsNullableDto
{
	public string TableName { get; set; }
	public string TableSchema { get; set; }
	public string ColumnName { get; set; }
	public string SourceIsNullable { get; set; }
	public string TargerIsNullable { get; set; }
}