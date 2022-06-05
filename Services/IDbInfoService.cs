namespace MsSQLDatabaseComparison.Services
{
	public interface IDbInfoService
	{
		Task<IReadOnlyList<DbTableDto>> GetTablesAsync(bool isSourceDb);
		Task<IReadOnlyList<DbColumnDto>> GetDbColumnsAsync(bool isSourceDb, string tableName = null);
		Task<IReadOnlyList<DbObjectDto>> GetFunctionsAsync(bool isSourceDb);
		Task<IReadOnlyList<DbObjectDto>> GetViewsAsync(bool isSourceDb);
		Task<IReadOnlyList<DbObjectDto>> GetStoredProceduresAsync(bool isSourceDb);
		Task<DashboardInfoDto> GetDashboardInfoDtoAsync();
		Task<IReadOnlyList<string>> GetAllTableNamesAsync();
		void RefreshAllCache();
	}
}