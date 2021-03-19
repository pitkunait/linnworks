using System;
using System.ComponentModel.DataAnnotations;

namespace LinnworksTechTest.Repositories.SalesRecords
{
    public class SalesRecord
    {
        public int Id { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string ItemType { get; set; } 
        public string SalesChannel { get; set; } 
        public char OrderPriority { get; set; } 
        public DateTime OrderDate { get; set; } 
        public int OrderId { get; set; } 
        public DateTime ShipDate { get; set; } 
        public int UnitsSold { get; set; } 
        public float UnitPrice { get; set; } 
        public float UnitCost { get; set; } 
        public float TotalRevenue { get; set; } 
        public float TotalCost { get; set; } 
        public float TotalProfit { get; set; }
    }
}