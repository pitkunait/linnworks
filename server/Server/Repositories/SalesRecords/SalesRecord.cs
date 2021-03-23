using System;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace Server.Repositories.SalesRecords
{
    public class SalesRecord
    {
        [Ignore]
        [Key]
        public int Id { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        [Name("Item Type")]
        public string ItemType { get; set; }
        [Name("Sales Channel")]
        public string SalesChannel { get; set; } 
        [Name("Order Priority")]
        public char OrderPriority { get; set; } 
        [Name("Order Date")]
        public DateTime OrderDate { get; set; } 
        [Name("Order ID")]
        public int OrderId { get; set; } 
        [Name("Ship Date")]
        public DateTime ShipDate { get; set; } 
        [Name("Units Sold")]
        public int UnitsSold { get; set; } 
        [Name("Unit Price")]
        public float UnitPrice { get; set; }
        [Name("Unit Cost")]
        public float UnitCost { get; set; }
        [Name("Total Revenue")]
        public float TotalRevenue { get; set; }
        [Name("Total Cost")]
        public float TotalCost { get; set; }
        [Name("Total Profit")]
        public float TotalProfit { get; set; }
    }
}