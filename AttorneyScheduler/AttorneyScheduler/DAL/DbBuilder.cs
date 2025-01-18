using Microsoft.Data.Sqlite;
using System.IO;

namespace AttorneyScheduler.DAL
{
    public static class DbBuilder
    {
        public static void BuildDatabase(string connectionString, string scriptFilePath)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder(connectionString);
            var filePath = connectionStringBuilder.DataSource;

            bool preExistingDb = File.Exists(filePath);

            if (!preExistingDb)
            {
                var directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (FileStream fs = File.Create(filePath))
                {
                    // create the db file
                }

                // Execute the script to create tables
                string script = File.ReadAllText(scriptFilePath);
                ExecuteScript(connectionString, script);
            }
        }

        private static void ExecuteScript(string connectionString, string script)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqliteCommand(script, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}