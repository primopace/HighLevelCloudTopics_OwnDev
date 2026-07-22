using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Order.API.Entities
{
  
    public class OrderView
    {
        [Key]
        public Guid OrderId { get; set; }

        public Guid CustomerId { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }

        public string PaymentStatus { get; set; }

        public string InventoryStatus { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
