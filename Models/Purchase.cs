using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }
        public int UserId { get; set; }
        public int MandarinId { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
