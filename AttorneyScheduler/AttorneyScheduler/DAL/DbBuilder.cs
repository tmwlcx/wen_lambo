using Microsoft.Data.Sqlite;

namespace AttorneyScheduler.DAL
{
    public static class DbBuilder
    {
        public static void BuildDatabase(string connectionString, string scriptFilePath)
        {
            var filePath = connectionString.Replace("Data Source=", "");
            bool preExistingDb;
            if (!File.Exists(filePath))
            {
                preExistingDb = false;
                File.Create(filePath);
            }
            else
            {
                preExistingDb = true;
            }

            if (preExistingDb == false)
            {
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
