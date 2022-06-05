namespace MsSQLDatabaseComparison.Services
{
	public class DbInfoService : IDbInfoService
	{
		private readonly MemoryCacheEntryOptions _cacheEntryOptions = new()
		{
			SlidingExpiration = TimeSpan.FromMinutes(15)
		};

		private readonly SourceDbContext _sourceDbContext;
		private readonly TargetDbContext _targetDbContext;

		private readonly IMemoryCache _memoryCache;

		public DbInfoService(SourceDbContext sourceDbContext, TargetDbContext targetDbContext, IMemoryCache memoryCache)
		{
			_sourceDbContext = sourceDbContext;
			_targetDbContext = targetDbContext;
			_memoryCache = memoryCache;
		}

		public async Task<IReadOnlyList<DbTableDto>> GetTablesAsync(bool isSourceDb)
		{
			var cacheKey = $"{nameof(GetTablesAsync)}.{isSourceDb}";
			if (_memoryCache.TryGetValue(cacheKey, out List<DbTableDto> dbTableDtos))
			{
				return dbTableDtos ?? new List<DbTableDto>();
			}

			dbTableDtos ??= new List<DbTableDto>();

			var sql = @"
			SELECT
				TABLE_NAME AS TableName,
				TABLE_SCHEMA AS TableSchema
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_TYPE = 'BASE TABLE'
			ORDER BY TABLE_SCHEMA, TABLE_NAME
			";

			var dbTables = new List<DbTable>();
			if (isSourceDb)
			{
				using var connection = _sourceDbContext.CreateConnection();
				dbTables = (await connection.QueryAsync<DbTable>(sql)).ToList();
			}
			else
			{
				using var connection = _targetDbContext.CreateConnection();
				dbTables = (await connection.QueryAsync<DbTable>(sql)).ToList();
			}

			dbTableDtos.AddRange(dbTables.Select(x => new DbTableDto { TableSchema = x.TableSchema, TableName = x.TableName }));
			_memoryCache.Set(cacheKey, dbTableDtos, _cacheEntryOptions);
			return dbTableDtos;
		}

		public async Task<IReadOnlyList<DbColumnDto>> GetDbColumnsAsync(bool isSourceDb, string tableName = null)
		{
			var cacheKey = $"{nameof(GetDbColumnsAsync)}.{isSourceDb}.{tableName}";
			if (_memoryCache.TryGetValue(cacheKey, out List<DbColumnDto> dbColumnDtos))
			{
				return dbColumnDtos ?? new List<DbColumnDto>();
			}

			dbColumnDtos ??= new List<DbColumnDto>();

			IDbConnection connection = null;

			if (isSourceDb)
			{
				connection = _sourceDbContext.CreateConnection();
			}
			else
			{
				connection = _targetDbContext.CreateConnection();
			}

			var sourceTables = await GetTablesAsync(isSourceDb: true);
			var targetTables = await GetTablesAsync(isSourceDb: false);

			var intersectTables = sourceTables.Where(x => targetTables.Any(y => x.TableSchema == y.TableSchema && x.TableName == y.TableName)).ToList();

			if (!string.IsNullOrEmpty(tableName))
			{
				intersectTables = intersectTables.Where(x => x.TableName == tableName).ToList();
			}

			foreach (var table in intersectTables)
			{
				var sql = $@"
				SELECT 
					TABLE_SCHEMA as TableSchema, 
					TABLE_NAME as TableName, 
					COLUMN_NAME as ColumnName, 
					DATA_TYPE as DataType, 
					COLUMN_DEFAULT as ColumnDefault, 
					IS_NULLABLE as IsNullable, 
					CHARACTER_MAXIMUM_LENGTH as CharacterMaximumLength, 
					NUMERIC_PRECISION as NumericPrecision, 
					NUMERIC_SCALE as NumericScale 
				FROM INFORMATION_SCHEMA.COLUMNS 
				WHERE 
					TABLE_SCHEMA = '{table.TableSchema}' 
					AND TABLE_NAME = '{table.TableName}' 
				ORDER BY COLUMN_NAME";

				var dbFields = (await connection.QueryAsync<DbColumn>(sql)).ToList();

				dbColumnDtos.AddRange(dbFields.Select(x => new DbColumnDto
				{
					CharacterMaximumLength = x.CharacterMaximumLength,
					ColumnDefault = x.ColumnDefault,
					ColumnName = x.ColumnName,
					DataType = x.DataType,
					IsNullable = x.IsNullable,
					NumericPrecision = x.NumericPrecision,
					NumericScale = x.NumericScale,
					TableName = x.TableName,
					TableSchema = x.TableSchema
				}));
			}

			connection?.Dispose();

			_memoryCache.Set(cacheKey, dbColumnDtos, _cacheEntryOptions);
			return dbColumnDtos;
		}

		public async Task<IReadOnlyList<DbObjectDto>> GetFunctionsAsync(bool isSourceDb)
		{
			var cacheKey = $"{nameof(GetFunctionsAsync)}.{isSourceDb}";
			if (_memoryCache.TryGetValue(cacheKey, out List<DbObjectDto> dbObjectDtos))
			{
				return dbObjectDtos ?? new List<DbObjectDto>();
			}

			dbObjectDtos ??= new List<DbObjectDto>();

			var sql = "SELECT name FROM sys.objects WHERE type = 'FN' or type ='TF' order by name";

			var functions = new List<DbObject>();

			if (isSourceDb)
			{
				using var connection = _sourceDbContext.CreateConnection();
				functions = (await connection.QueryAsync<DbObject>(sql)).ToList();
			}
			else
			{
				using var connection = _targetDbContext.CreateConnection();
				functions = (await connection.QueryAsync<DbObject>(sql)).ToList();
			}

			dbObjectDtos.AddRange(functions.Select(x => new DbObjectDto { Name = x.Name }));
			_memoryCache.Set(cacheKey, dbObjectDtos, _cacheEntryOptions);
			return dbObjectDtos;
		}

		public async Task<IReadOnlyList<DbObjectDto>> GetViewsAsync(bool isSourceDb)
		{
			var cacheKey = $"{nameof(GetViewsAsync)}.{isSourceDb}";
			if (_memoryCache.TryGetValue(cacheKey, out List<DbObjectDto> dbObjectDtos))
			{
				return dbObjectDtos ?? new List<DbObjectDto>();
			}

			dbObjectDtos ??= new List<DbObjectDto>();

			var sql = "SELECT name FROM sys.objects WHERE type = 'V' order by name";

			var views = new List<DbObject>();

			if (isSourceDb)
			{
				using var connection = _sourceDbContext.CreateConnection();
				views = (await connection.QueryAsync<DbObject>(sql)).ToList();
			}
			else
			{
				using var connection = _targetDbContext.CreateConnection();
				views = (await connection.QueryAsync<DbObject>(sql)).ToList();
			}

			dbObjectDtos.AddRange(views.Select(x => new DbObjectDto { Name = x.Name }));
			_memoryCache.Set(cacheKey, dbObjectDtos, _cacheEntryOptions);
			return dbObjectDtos;
		}

		public async Task<IReadOnlyList<DbObjectDto>> GetStoredProceduresAsync(bool isSourceDb)
		{
			var cacheKey = $"{nameof(GetStoredProceduresAsync)}.{isSourceDb}";
			if (_memoryCache.TryGetValue(cacheKey, out List<DbObjectDto> dbObjectDtos))
			{
				return dbObjectDtos ?? new List<DbObjectDto>();
			}

			dbObjectDtos ??= new List<DbObjectDto>();

			var sql = "SELECT name FROM sys.objects WHERE type = 'P' order by name";

			var storedProcedures = new List<DbObject>();

			if (isSourceDb)
			{
				using var connection = _sourceDbContext.CreateConnection();
				storedProcedures = (await connection.QueryAsync<DbObject>(sql)).ToList();
			}
			else
			{
				using var connection = _targetDbContext.CreateConnection();
				storedProcedures = (await connection.QueryAsync<DbObject>(sql)).ToList();
			}

			dbObjectDtos.AddRange(storedProcedures.Select(x => new DbObjectDto { Name = x.Name }));
			_memoryCache.Set(cacheKey, dbObjectDtos, _cacheEntryOptions);
			return dbObjectDtos;
		}

		public async Task<DashboardInfoDto> GetDashboardInfoDtoAsync()
		{
			var cacheKey = $"{nameof(GetDashboardInfoDtoAsync)}";
			if (_memoryCache.TryGetValue(cacheKey, out DashboardInfoDto dashboardInfoDto))
			{
				return dashboardInfoDto ?? new DashboardInfoDto { };
			}

			var tableCountSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
			var columnCountSql = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS";
			var functionCountSql = "SELECT COUNT(*) FROM sys.objects WHERE type = 'FN' or type ='TF'";
			var viewCountSql = "SELECT COUNT(*) FROM sys.objects WHERE type = 'V'";
			var storedProcedureCountSql = "SELECT COUNT(*) FROM sys.objects WHERE type = 'P'";

			using var sourceConnection = _sourceDbContext.CreateConnection();
			var sourceTableCount = await sourceConnection.QuerySingleOrDefaultAsync<int>(tableCountSql);
			var sourceColumnCount = await sourceConnection.QuerySingleOrDefaultAsync<int>(columnCountSql);
			var sourceFunctionCount = await sourceConnection.QuerySingleOrDefaultAsync<int>(functionCountSql);
			var sourceViewCount = await sourceConnection.QuerySingleOrDefaultAsync<int>(viewCountSql);
			var sourceStoredProcedureCount = await sourceConnection.QuerySingleOrDefaultAsync<int>(storedProcedureCountSql);

			using var targetConnection = _targetDbContext.CreateConnection();
			var targetTableCount = await targetConnection.QuerySingleOrDefaultAsync<int>(tableCountSql);
			var targetColumnCount = await targetConnection.QuerySingleOrDefaultAsync<int>(columnCountSql);
			var targetFunctionCount = await targetConnection.QuerySingleOrDefaultAsync<int>(functionCountSql);
			var targetViewCount = await targetConnection.QuerySingleOrDefaultAsync<int>(viewCountSql);
			var targetStoredProcedureCount = await targetConnection.QuerySingleOrDefaultAsync<int>(storedProcedureCountSql);

			dashboardInfoDto = new DashboardInfoDto
			{
				SourceTableCount = sourceTableCount,
				SourceColumnCount = sourceColumnCount,
				SourceFunctionCount = sourceFunctionCount,
				SourceViewCount = sourceViewCount,
				SourceStoredProcedureCount = sourceStoredProcedureCount,
				TargetTableCount = targetTableCount,
				TargetColumnCount = targetColumnCount,
				TargetFunctionCount = targetFunctionCount,
				TargetViewCount = targetViewCount,
				TargetStoredProcedureCount = targetStoredProcedureCount
			};

			_memoryCache.Set(cacheKey, dashboardInfoDto, _cacheEntryOptions);
			return dashboardInfoDto;
		}

		public async Task<IReadOnlyList<string>> GetAllTableNamesAsync()
		{
			var cacheKey = $"{nameof(GetAllTableNamesAsync)}";
			if (_memoryCache.TryGetValue(cacheKey, out List<string> tableNames))
			{
				return tableNames ?? new List<string> { };
			}

			tableNames ??= new List<string>();

			var sourceTables = await GetTablesAsync(isSourceDb: true);
			var targetTables = await GetTablesAsync(isSourceDb: false);

			tableNames.AddRange(sourceTables.Select(x => x.TableName));
			tableNames.AddRange(targetTables.Select(x => x.TableName));

			tableNames = tableNames.Distinct().ToList();

			_memoryCache.Set(cacheKey, tableNames, _cacheEntryOptions);
			return tableNames;
		}

		/// <summary>
		/// We are currently use MemoryCache for caching. If you decide to use Distributed Cache like redis, azure etc. this method loses its functionality.
		/// </summary>
		public void RefreshAllCache()
		{
			if (_memoryCache is MemoryCache memoryCache)
			{
				memoryCache.Compact(1.0);
			}
		}
	}
}