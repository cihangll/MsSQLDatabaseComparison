namespace MsSQLDatabaseComparison.DbContexts;

public class SourceDbContext
{
	private readonly IConfiguration _configuration;
	private readonly string _connectionString;
	public SourceDbContext(IConfiguration configuration)
	{
		_configuration = configuration;
		_connectionString = _configuration.GetConnectionString("SourceConnectionString");
	}
	public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}