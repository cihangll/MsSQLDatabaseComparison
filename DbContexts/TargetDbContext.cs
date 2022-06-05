namespace MsSQLDatabaseComparison.DbContexts;

public class TargetDbContext
{
	private readonly IConfiguration _configuration;
	private readonly string _connectionString;
	public TargetDbContext(IConfiguration configuration)
	{
		_configuration = configuration;
		_connectionString = _configuration.GetConnectionString("TargetConnectionString");
	}
	public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}