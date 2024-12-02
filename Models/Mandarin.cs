using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineAuction.Models
{
    public class Mandarin
    {
        public int Id { get; set; }
        public string? ImageUrl {  get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsSpoiled { get; set; }
        public Decimal Price { get; set; }
        public bool IsSold { get; set; } = false;
        public Decimal FinalPrice { get; set; }
    }
}
