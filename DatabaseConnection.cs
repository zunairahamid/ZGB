using System.Data.SqlClient;  // Change from MySqlConnector

public class DatabaseConnection
{
    private readonly string _connectionString;

    public DatabaseConnection()
    {
        _connectionString = "Data Source=DESKTOP-G7Q5BQ4;Initial Catalog=marketdb;Integrated Security=True;Pooling=False;Encrypt=True;Trust Server Certificate=True";
    }

    public SqlConnection GetConnection()  // Changed return type
    {
        return new SqlConnection(_connectionString);
    }
}