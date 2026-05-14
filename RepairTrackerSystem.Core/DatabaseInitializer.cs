using System.Data.SQLite;

namespace RepairTrackerSystem.Core
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                // Create Repairs table if it doesn't exist
                var cmd = new SQLiteCommand(@"
                CREATE TABLE IF NOT EXISTS Repairs (
                    RepairID TEXT PRIMARY KEY,
                    CustomerID TEXT,
                    DeviceID TEXT,
                    TechnicianID TEXT,
                    Status TEXT,
                    Issue TEXT,
                    Cost REAL,
                    DateReceived TEXT,
                    DateUpdated TEXT
                );", conn);

                cmd.ExecuteNonQuery();

                // Add Cost column if missing
                try
                {
                    var alterCmd = new SQLiteCommand(
                        "ALTER TABLE Repairs ADD COLUMN Cost REAL DEFAULT 0",
                        conn);

                    alterCmd.ExecuteNonQuery();
                }
                catch
                {
                    // Column already exists
                }
            }
        }
    }
}