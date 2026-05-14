using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace RepairTrackerSystem.Core
{
    public static class DatabaseService
    {
        private static readonly object _writeLock = new object();
        // ================= ID GENERATORS =================
        public static string GenerateCustomerId() => "C-" + DateTime.Now.Ticks.ToString().Substring(10);
        public static string GenerateDeviceId() => "D-" + DateTime.Now.Ticks.ToString().Substring(10);
        public static string GenerateTechnicianId() => "T-" + DateTime.Now.Ticks.ToString().Substring(10);
        public static string GenerateRepairId() => "R-" + DateTime.Now.Ticks.ToString().Substring(10);

        // ================= CUSTOMERS =================
        public static List<Customer> GetCustomers(string filter = null)
        {
            var list = new List<Customer>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM Customers";
                if (!string.IsNullOrWhiteSpace(filter))
                    query += " WHERE Name LIKE @filter";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    if (filter != null)
                        cmd.Parameters.AddWithValue("@filter", "%" + filter + "%");

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list.Add(new Customer
                        {
                            ID = reader["ID"].ToString(),
                            Name = reader["Name"].ToString(),
                            Contact = reader["Contact"].ToString(),
                            Address = reader["Address"].ToString()
                        });
                    }
                }
            }

            return list;
        }

        public static Customer GetCustomer(string id)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand("SELECT * FROM Customers WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                var r = cmd.ExecuteReader();
                if (r.Read())
                {
                    return new Customer
                    {
                        ID = r["ID"].ToString(),
                        Name = r["Name"].ToString(),
                        Contact = r["Contact"].ToString(), // ? was missing
                        Address = r["Address"].ToString()  // ? was missing
                    };
                }
            }
            return null;
        }

        public static void AddCustomer(Customer c)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand("INSERT INTO Customers VALUES (@id,@name,@contact,@address)", conn);
                cmd.Parameters.AddWithValue("@id", GenerateCustomerId());
                cmd.Parameters.AddWithValue("@name", c.Name);
                cmd.Parameters.AddWithValue("@contact", c.Contact);
                cmd.Parameters.AddWithValue("@address", c.Address);
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateCustomer(Customer c)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand("UPDATE Customers SET Name=@name, Contact=@contact, Address=@address WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", c.ID);
                cmd.Parameters.AddWithValue("@name", c.Name);
                cmd.Parameters.AddWithValue("@contact", c.Contact);
                cmd.Parameters.AddWithValue("@address", c.Address);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteCustomer(string id)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SQLiteCommand("DELETE FROM Customers WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        // ================= DEVICES =================
        public static List<Device> GetDevices(string customerId = null)
        {
            var list = new List<Device>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM Devices";
                if (!string.IsNullOrEmpty(customerId))
                    query += " WHERE CustomerID=@cid";

                var cmd = new SQLiteCommand(query, conn);

                if (customerId != null)
                    cmd.Parameters.AddWithValue("@cid", customerId);

                var r = cmd.ExecuteReader();

                while (r.Read())
                {
                    list.Add(new Device
                    {
                        ID = r["ID"].ToString(),
                        Type = r["Type"].ToString(),
                        Brand = r["Brand"].ToString(),
                        Model = r["Model"].ToString(),
                        CustomerID = r["CustomerID"].ToString()
                    });
                }
            }

            return list;
        }

        public static Device GetDevice(string id)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand("SELECT * FROM Devices WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                var r = cmd.ExecuteReader();
                if (r.Read())
                {
                    return new Device
                    {
                        ID = r["ID"].ToString(),
                        Type = r["Type"].ToString(),
                        Brand = r["Brand"].ToString(),
                        Model = r["Model"].ToString(),
                        CustomerID = r["CustomerID"].ToString()
                    };
                }
            }
            return null;
        }

        public static void AddDevice(Device d)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand("INSERT INTO Devices VALUES (@id,@type,@brand,@model,@cid)", conn);
                cmd.Parameters.AddWithValue("@id", GenerateDeviceId());
                cmd.Parameters.AddWithValue("@type", d.Type);
                cmd.Parameters.AddWithValue("@brand", d.Brand);
                cmd.Parameters.AddWithValue("@model", d.Model);
                cmd.Parameters.AddWithValue("@cid", d.CustomerID);
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateDevice(Device d)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand("UPDATE Devices SET Type=@type, Brand=@brand, Model=@model, CustomerID=@cid WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", d.ID);
                cmd.Parameters.AddWithValue("@type", d.Type);
                cmd.Parameters.AddWithValue("@brand", d.Brand);
                cmd.Parameters.AddWithValue("@model", d.Model);
                cmd.Parameters.AddWithValue("@cid", d.CustomerID);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteDevice(string id)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SQLiteCommand("DELETE FROM Devices WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        // ================= TECHNICIANS =================
        public static List<Technician> GetTechnicians()
        {
            var list = new List<Technician>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand("SELECT * FROM Technicians", conn);
                var r = cmd.ExecuteReader();

                while (r.Read())
                {
                    list.Add(new Technician
                    {
                        ID = r["ID"]?.ToString(),
                        Name = r["Name"]?.ToString(),
                        Specialty = r["Specialty"]?.ToString(),
                        Contact = r["Contact"]?.ToString()
                    });
                }
            }

            return list;
        }

        public static Technician GetTechnician(string id)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand("SELECT * FROM Technicians WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                var r = cmd.ExecuteReader();
                if (r.Read())
                {
                    return new Technician
                    {
                        ID = r["ID"]?.ToString(),
                        Name = r["Name"]?.ToString(),
                        Specialty = r["Specialty"]?.ToString(),
                        Contact = r["Contact"]?.ToString()
                    };
                }
            }
            return null;
        }

        public static void AddTechnician(Technician t)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand(
                    "INSERT INTO Technicians (ID, Name, Specialty, Contact) VALUES (@id,@name,@spec,@contact)", conn);

                cmd.Parameters.AddWithValue("@id", GenerateTechnicianId());
                cmd.Parameters.AddWithValue("@name", t.Name);
                cmd.Parameters.AddWithValue("@spec", t.Specialty);
                cmd.Parameters.AddWithValue("@contact", t.Contact);

                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateTechnician(Technician t)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand(
                    "UPDATE Technicians SET Name=@name, Specialty=@spec, Contact=@contact WHERE ID=@id", conn);

                cmd.Parameters.AddWithValue("@id", t.ID);
                cmd.Parameters.AddWithValue("@name", t.Name);
                cmd.Parameters.AddWithValue("@spec", t.Specialty);
                cmd.Parameters.AddWithValue("@contact", t.Contact);

                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteTechnician(string id)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand("DELETE FROM Technicians WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        // ================= REPAIRS =================
        public static List<Repair> GetRepairs(string filter = null, string status = null)
        {
            var list = new List<Repair>();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM Repairs WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(filter))
                    query += " AND RepairID LIKE @filter";

                if (!string.IsNullOrWhiteSpace(status))
                    query += " AND Status=@status";

                var cmd = new SQLiteCommand(query, conn);

                if (filter != null)
                    cmd.Parameters.AddWithValue("@filter", "%" + filter + "%");

                if (status != null)
                    cmd.Parameters.AddWithValue("@status", status);

                var r = cmd.ExecuteReader();

                while (r.Read())
                {
                    DateTime.TryParse(r["DateReceived"]?.ToString(), out DateTime received);

                    DateTime? updated = null;
                    if (DateTime.TryParse(r["DateUpdated"]?.ToString(), out DateTime temp))
                        updated = temp;

                    list.Add(new Repair
                    {
                        RepairID = r["RepairID"]?.ToString(),
                        CustomerID = r["CustomerID"]?.ToString(),
                        DeviceID = r["DeviceID"]?.ToString(),
                        TechnicianID = r["TechnicianID"]?.ToString(),
                        Status = r["Status"]?.ToString(),
                        Issue = r["Issue"]?.ToString(),
                        Cost = r["Cost"] != DBNull.Value ? Convert.ToDecimal(r["Cost"]) : 0,
                        DateReceived = received,
                        DateUpdated = updated
                    });
                }
            }

            return list;
        }

        public static Repair GetRepair(string id)
{
    using (var conn = Database.GetConnection())
    {
        conn.Open();

        var cmd = new SQLiteCommand("SELECT * FROM Repairs WHERE RepairID=@id", conn);
        cmd.Parameters.AddWithValue("@id", id);

        var r = cmd.ExecuteReader();
        if (r.Read())
        {
            DateTime.TryParse(r["DateReceived"]?.ToString(), out DateTime received);
            DateTime? updated = null;
            if (DateTime.TryParse(r["DateUpdated"]?.ToString(), out DateTime temp))
                updated = temp;

            return new Repair
            {
                RepairID     = r["RepairID"]?.ToString(),
                CustomerID   = r["CustomerID"]?.ToString(),
                DeviceID     = r["DeviceID"]?.ToString(),
                TechnicianID = r["TechnicianID"]?.ToString(),
                Status       = r["Status"]?.ToString(),
                Issue        = r["Issue"]?.ToString(),
                Cost         = decimal.TryParse(r["Cost"]?.ToString(), out decimal c) ? c : 0,
                DateReceived = received,
                DateUpdated  = updated
            };
        }
    }
    return null;
}

        public static void UpdateRepair(Repair r)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand(@"
            UPDATE Repairs 
            SET Status      = @status,
                TechnicianID = @tid,
                Cost         = @cost,
                Issue        = @issue,
                DateUpdated  = @updated
            WHERE RepairID = @id", conn);

                cmd.Parameters.AddWithValue("@id", r.RepairID);
                cmd.Parameters.AddWithValue("@status", r.Status);
                cmd.Parameters.AddWithValue("@tid", r.TechnicianID);
                cmd.Parameters.AddWithValue("@cost", (double)r.Cost);
                cmd.Parameters.AddWithValue("@issue", r.Issue ?? "");
                cmd.Parameters.AddWithValue("@updated", r.DateUpdated?.ToString());

                cmd.ExecuteNonQuery();
            }
        }
        public static void AddRepair(Repair r)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                var cmd = new SQLiteCommand(@"
            INSERT INTO Repairs 
            (RepairID, CustomerID, DeviceID, TechnicianID, Status, Issue, Cost, DateReceived, DateUpdated)
            VALUES 
            (@id, @cid, @did, @tid, @status, @issue, @cost, @date, @updated)", conn);

                cmd.Parameters.AddWithValue("@id", r.RepairID);
                cmd.Parameters.AddWithValue("@cid", r.CustomerID);
                cmd.Parameters.AddWithValue("@did", r.DeviceID);
                cmd.Parameters.AddWithValue("@tid", r.TechnicianID);
                cmd.Parameters.AddWithValue("@status", r.Status);
                cmd.Parameters.AddWithValue("@issue", r.Issue ?? "");
                cmd.Parameters.AddWithValue("@cost", (double)r.Cost);
                cmd.Parameters.AddWithValue("@date", r.DateReceived.ToString());
                cmd.Parameters.AddWithValue("@updated", r.DateUpdated?.ToString());

                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteRepair(string id)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SQLiteCommand("DELETE FROM Repairs WHERE RepairID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}