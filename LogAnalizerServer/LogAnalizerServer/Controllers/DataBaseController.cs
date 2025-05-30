using LogAnalizerServer.Data;
using LogAnalizerServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LogAnalizerServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataBaseController : ControllerBase
    {
        private readonly string _databaseFolder;
        private readonly string _serverName;
        private readonly string _defaultDatabase;

        public DataBaseController(IOptions<DatabaseSettings> settings)
        {
            _databaseFolder = Path.Combine(Directory.GetCurrentDirectory(), "Databases");
            _serverName = settings.Value.Server;
            _defaultDatabase = settings.Value.Database;

            if (!Directory.Exists(_databaseFolder))
                Directory.CreateDirectory(_databaseFolder);
        }

      
        
        
   
        
        
        private void EnsureDatabaseIsReady()
        {
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<LogAnalizerServerDbContext>();

                if (DatabaseConnectionManager.CurrentMode == DatabaseMode.Sqlite)
                    optionsBuilder.UseSqlite(DatabaseConnectionManager.CurrentConnectionString);
                else
                    optionsBuilder.UseSqlServer(DatabaseConnectionManager.CurrentConnectionString);

                using var context = new LogAnalizerServerDbContext(optionsBuilder.Options);

                if (DatabaseConnectionManager.CurrentMode == DatabaseMode.Sqlite)
                {
                    context.Database.EnsureCreated(); 
                }
                else
                {
                    context.Database.Migrate(); 
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при EnsureDatabaseIsReady: {ex.Message}", ex);
            }
        }
        
     
        
        
        
      
        
        

       
        
        [HttpPost("set-server")]
        public ActionResult SetSqlServer([FromBody] string serverName)
        {
            if (string.IsNullOrWhiteSpace(serverName))
                return BadRequest("Server name is required.");

            var defaultDbName = "master";
            var connectionString = $"Server={serverName};Database={defaultDbName};Trusted_Connection=True;TrustServerCertificate=True;";

            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to connect to server '{serverName}': {ex.Message}");
            }

            DatabaseConnectionManager.UseSqlServer(serverName, defaultDbName, "", "");
            return Ok($"Server set to '{serverName}'");
        }
        

     
        
        [HttpGet("list")]
        public ActionResult<List<string>> ListDatabases()
        {
            var dbNames = new List<string>();

            try
            {
                using var connection = new SqlConnection(DatabaseConnectionManager.CurrentConnectionString);
                connection.Open();

                var cmd = new SqlCommand("SELECT name FROM sys.databases WHERE name NOT IN ('master','tempdb','model','msdb')", connection);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    dbNames.Add(reader.GetString(0));
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при получении списка баз: {ex.Message}");
            }

            return Ok(dbNames);
        }
        

  
        
        
        [HttpPost("create")]
        public ActionResult CreateDatabase([FromQuery] string dbName)
        {
            try
            {
                Console.WriteLine($"Received DB name: '{dbName}'");
                if (string.IsNullOrWhiteSpace(dbName))
                    return BadRequest("Database name is empty!");

                var builder = new SqlConnectionStringBuilder(DatabaseConnectionManager.CurrentConnectionString)
                {
                    InitialCatalog = "master"
                };

                using var connection = new SqlConnection(builder.ConnectionString);
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = $"IF DB_ID('{dbName}') IS NULL CREATE DATABASE [{dbName}]";
                cmd.ExecuteNonQuery();

                builder.InitialCatalog = dbName;
                DatabaseConnectionManager.SetConnectionString(builder.ConnectionString);

                EnsureDatabaseIsReady();

                return Ok($"DB '{dbName}' created and ready.");
            }
            catch (Exception ex)
            {
                return BadRequest($"SQL Server DB creation failed: {ex.Message}");
            }
        }
        
        
        [HttpPost("select")]
        public ActionResult SelectDatabase(string dbName)
        {
            var builder = new SqlConnectionStringBuilder(DatabaseConnectionManager.CurrentConnectionString)
            {
                InitialCatalog = dbName
            };

            DatabaseConnectionManager.SetConnectionString(builder.ConnectionString);
            EnsureDatabaseIsReady();

            return Ok($"DB '{dbName}' selected.");
        }

      
        
        [HttpDelete("delete")]
        public ActionResult DeleteDatabase(string dbName)
        {
            try
            {
              
                var builder = new SqlConnectionStringBuilder(DatabaseConnectionManager.CurrentConnectionString)
                {
                    InitialCatalog = "master"
                };

                using var connection = new SqlConnection(builder.ConnectionString);
                connection.Open();

                var cmd = new SqlCommand($@"
            ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
            DROP DATABASE [{dbName}];", connection);

                cmd.ExecuteNonQuery();

                return Ok($"DB '{dbName}' deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest($"SQL Server DB deletion failed: {ex.Message}");
            }
        }
        
        
     
        

        // ---------------- SQLITE ----------------

      
        
        
        [HttpPost("select-sqlite")]
        public IActionResult SelectSqlite([FromBody] SqliteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FilePath))
                return BadRequest("File path is required.");

            var fullPath = Path.Combine(_databaseFolder, request.FilePath);

            if (!System.IO.File.Exists(fullPath))
                return NotFound($"Database '{request.FilePath}' not found.");

            try
            {
                DatabaseConnectionManager.UseSqlite(request.FilePath);
                EnsureDatabaseIsReady();
                return Ok($"Database '{request.FilePath}' selected.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to switch to SQLite: {ex.Message}");
            }
        }
        
        
        [HttpGet("list-sqlite")]
        public IActionResult ListSqliteDatabases()
        {
            var dbFiles = Directory.GetFiles(_databaseFolder)
                .Select(f => Path.GetFileName(f))
                .ToList();

            return Ok(dbFiles);
        }

      

        [HttpPost("create-sqlite")]
        public IActionResult CreateSqlite([FromBody] SqliteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FilePath))
                return BadRequest("File path is required.");

            var fullPath = Path.Combine(_databaseFolder, request.FilePath);

            try
            {
                if (!System.IO.File.Exists(fullPath))
                    System.IO.File.Create(fullPath).Dispose();

                DatabaseConnectionManager.UseSqlite(request.FilePath);
                EnsureDatabaseIsReady();

                return Ok($"SQLite DB '{request.FilePath}' created.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"SQLite creation failed: {ex.Message}");
            }
        }

        /*
        [HttpPost("delete-sqlite")]
        public IActionResult DeleteSqlite([FromBody] SqliteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FilePath))
                return BadRequest("File path is required.");

            var fullPath = Path.Combine(_databaseFolder, request.FilePath);

            try
            {
                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);

                return Ok($"SQLite DB '{request.FilePath}' deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"SQLite deletion failed: {ex.Message}");
            }
        }*/
        
        
        [HttpPost("delete-sqlite")]
        public IActionResult DeleteSqlite([FromBody] SqliteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FilePath))
                return BadRequest("File path is required.");

            try
            {
                // Получаем только имя файла (на случай, если пришёл полный путь)
                var fileName = Path.GetFileName(request.FilePath);

                // Собираем абсолютный путь на сервере
                var fullPath = Path.Combine(_databaseFolder, fileName);

                Console.WriteLine("[DELETE SQLITE] Request.FilePath: " + request.FilePath);
                Console.WriteLine("[DELETE SQLITE] FileName: " + fileName);
                Console.WriteLine("[DELETE SQLITE] Resolved full path: " + fullPath);

                if (!System.IO.File.Exists(fullPath))
                {
                    Console.WriteLine("[DELETE SQLITE] File not found.");
                    return NotFound($"File '{fileName}' does not exist.");
                }

                System.IO.File.Delete(fullPath);
                Console.WriteLine("[DELETE SQLITE] File deleted.");

                return Ok($"SQLite DB '{fileName}' deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DELETE SQLITE] Exception: " + ex.Message);
                return StatusCode(500, $"SQLite deletion failed: {ex.Message}");
            }
        }
        
        
        
        
        [HttpPost("set-server-auth")]
        public ActionResult SetSqlServerWithAuth([FromBody] SqlServerAuthRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Server) ||
                string.IsNullOrWhiteSpace(request.User) ||
                string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Incomplete connection details (server, user or password missing).");

            var db = string.IsNullOrWhiteSpace(request.Database) ? "master" : request.Database;

            try
            {
                var connectionString = $"Server={request.Server};Database={db};User Id={request.User};Password={request.Password};TrustServerCertificate=True;Encrypt=True;";
                using var connection = new SqlConnection(connectionString);
                connection.Open(); 
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to connect: {ex.Message}");
            }

            DatabaseConnectionManager.UseSqlServer(request.Server, db, request.User, request.Password);
            return Ok("Connected to remote SQL Server.");
        }
        
        
        
        
        
        
    }
}