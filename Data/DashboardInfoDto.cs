namespace MsSQLDatabaseComparison.Data;

public class DashboardInfoDto
{
	public int SourceTableCount { get; internal set; }
	public int SourceColumnCount { get; internal set; }
	public int SourceFunctionCount { get; internal set; }
	public int SourceViewCount { get; internal set; }
	public int SourceStoredProcedureCount { get; internal set; }
	public int TargetTableCount { get; internal set; }
	public int TargetColumnCount { get; internal set; }
	public int TargetFunctionCount { get; internal set; }
	public int TargetViewCount { get; internal set; }
	public int TargetStoredProcedureCount { get; internal set; }
}