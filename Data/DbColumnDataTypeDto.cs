namespace MsSQLDatabaseComparison.Data;

public class DbColumnDefaultDto
{
	public string TableName { get; set; }
	public string TableSchema { get; set; }
	public string ColumnName { get; set; }
	public string SourceColumnDefault { get; set; }
	public string TargetColumnDefault { get; set; }
}
