using System.Data;
using System.IO;
using System.Windows.Documents;
using Microsoft.Data.SqlClient;

namespace CourseProject;

public class DataBase
{
    public static List<string> _tables = new List<string>(); 
    private static string _connectionString =
        "Server=YAKOVLAPTOP;Database=CourseWork;TrustServerCertificate=True; Trusted_Connection=True;";
    private static string _conForCreate = "Server=YAKOVLAPTOP;Database=master;TrustServerCertificate=True; Trusted_Connection=True;";

    public static int CreateDataBase()
    {
        using (SqlConnection connection = new SqlConnection(_conForCreate))
        {
            connection.Open();
            string sqlExpression = "CREATE DATABASE CourseWork";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            return command.ExecuteNonQuery();
        }
    }

    public static void CreateDB()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command =
                new SqlCommand("CREATE TABLE datatables (Id INT NOT NULL IDENTITY, Name NVARCHAR(100) NOT NULL )");
            command.ExecuteNonQuery();
        }
    }
    
    public static void CreateNewTable(string name)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string sqlExpression = $"CREATE TABLE {name}_d (Id INT PRIMARY KEY IDENTITY, Date NVARCHAR(100) NOT NULL );" +
                                   $"CREATE TABLE {name}_a (Id INT PRIMARY KEY identity);";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            AddTables(name);
            GetTables();
            command.ExecuteNonQuery();
        }
    }

    public static void DeleteTable(string name)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string sqlExpression = $"DROP TABLE {name}_d;" +
                                   $"DROP TABLE {name}_a;";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            DeleteTables(name);
            GetTables();
            command.ExecuteNonQuery();
        }
    }

    public static DataTable ShowTable(string name)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        SqlCommand command = new SqlCommand($"SELECT * FROM {name};",connection);
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
        DataTable dataTable = new DataTable();
        sqlDataAdapter.Fill(dataTable);
        return dataTable;
    }
    
    

    public static void DropTableA(string name)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string sqlExpression = $"DROP TABLE {name}_a;";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            _tables.Remove(name);
            command.ExecuteNonQuery();
        }
    }

    public static void SaveTable(string name,DataTable dataTable)
    {
        SqlConnection connection = new SqlConnection(_connectionString);
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter($"SELECT * FROM {name}",connection);
        new SqlCommandBuilder(sqlDataAdapter);
        sqlDataAdapter.Update(dataTable);
    }

    public static void GetTables()
    {
        _tables = new List<string>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT datatables.Name FROM datatables",connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    _tables.Add(reader.GetString(0));
                }
            }
        }
    }

    public static void AddTables(string name)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand($"INSERT datatables VALUES ('{name}');",connection);
            command.ExecuteNonQuery();
        }
    }
    
    public static void DeleteTables(string name)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand($"DELETE datatables WHERE Name = '{name}';",connection);
            command.ExecuteNonQuery();
        }
    }

    public static void GenTableA(string name)
    {
        DropTableA(name);
        List<string> list = new List<string>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string sqlExpression = $"SELECT * FROM {name}_d";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read()) // построчно считываем данные
                {
                    string date = reader.GetString(1);
                    list.Add(date);
                }
            }
        }

        string str = $"CREATE TABLE {name}_a (Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(100)";
        foreach (var l in list)
        {
            str += $", d{l} NVARCHAR(100)";
        }

        str += ");";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(str, connection);
            _tables.Add(name);
            command.ExecuteNonQuery();
        }
        
    }
    
    
}