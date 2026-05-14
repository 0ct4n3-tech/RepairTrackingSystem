using System;
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
                InitializeSchema();  // Only creates missing tables
            }
            else
            {
                // Ensure schema is initialized even if DB exists (creates missing tables only)
                EnsureTablesExist();
            }

            return new SQLiteConnection(connectionString);
        }

        // ═══════════════════════════════════════════════════════════════════
        // INITIALIZE DATABASE SCHEMA (for new databases)
        // ═══════════════════════════════════════════════════════════════════
        private static void InitializeSchema()
        {
            EnsureTablesExist();
        }

        // ═══════════════════════════════════════════════════════════════════
        // ENSURE ALL REQUIRED TABLES EXIST (creates only missing tables)
        // ═══════════════════════════════════════════════════════════════════
        private static void EnsureTablesExist()
        {
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string[] schemas = new[]
                    {
                        // Customers Table
                        @"CREATE TABLE IF NOT EXISTS Customers (
                            ID TEXT PRIMARY KEY,
                            Name TEXT NOT NULL,
                            Contact TEXT,
                            Address TEXT
                        );",

                        // Devices Table
                        @"CREATE TABLE IF NOT EXISTS Devices (
                            ID TEXT PRIMARY KEY,
                            Type TEXT NOT NULL,
                            Brand TEXT,
                            Model TEXT
                        );",

                        // Technicians Table
                        @"CREATE TABLE IF NOT EXISTS Technicians (
                            ID TEXT PRIMARY KEY,
                            Name TEXT NOT NULL,
                            Specialty TEXT,
                            Contact TEXT
                        );",

                        // Repairs Table (Main table for analytics)
                        @"CREATE TABLE IF NOT EXISTS Repairs (
                            RepairID TEXT PRIMARY KEY,
                            CustomerID TEXT NOT NULL,
                            DeviceID TEXT NOT NULL,
                            TechnicianID TEXT,
                            Status TEXT NOT NULL,
                            Issue TEXT,
                            Cost REAL,
                            DateReceived DATETIME NOT NULL,
                            DateUpdated DATETIME,
                            FOREIGN KEY(CustomerID) REFERENCES Customers(ID),
                            FOREIGN KEY(DeviceID) REFERENCES Devices(ID),
                            FOREIGN KEY(TechnicianID) REFERENCES Technicians(ID)
                        );"
                    };

                    foreach (var schema in schemas)
                    {
                        using (var cmd = new SQLiteCommand(schema, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Schema initialization error: {ex.Message}");
            }
        }
    }
}