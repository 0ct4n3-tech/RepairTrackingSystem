using System;
using System.Collections.Generic;

namespace RepairTrackerSystem.Core
{
        public class Customer
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Contact { get; set; }
            public string Address { get; set; }

            public override string ToString() => Name;
        }
    }

public class Device
{
    public string ID { get; set; }
    public string Type { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string CustomerID { get; set; }

    public override string ToString() => $"{Type} - {Brand} {Model}";
}

public class Technician
{
    public string ID { get; set; }
    public string Name { get; set; }
    public string Specialty { get; set; }
    public string Contact { get; set; }

    public override string ToString() => Name;
}

public class Repair
{
    public string RepairID { get; set; }
    public string CustomerID { get; set; }
    public string DeviceID { get; set; }
    public string TechnicianID { get; set; }
    public string Status { get; set; }
    public string Issue { get; set; }
    public decimal Cost { get; set; }
    public DateTime DateReceived { get; set; }
    public DateTime? DateUpdated { get; set; }
}

