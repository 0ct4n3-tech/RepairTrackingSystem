using System.Data.SQLite;
using System.IO;


namespace RepairTrackerSystem.Core
{
    public static class Database
    {
        private static string dbFile = "repairtracker.db";
        // Enable WAL and pooling to reduce locking issues
        private static string connectionString = $"Data Source={dbFile};Version=3;Pooling=True;Max Pool Size=100;Journal Mode=WAL;Foreign Keys=True;";

        public static SQLiteConnection GetConnection()
        {
            if (!File.Exists(dbFile))
            {
                SQLiteConnection.CreateFile(dbFile);
            }

            return new SQLiteConnection(connectionString);
        }
    }
}