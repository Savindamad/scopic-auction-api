using System;

#nullable disable

namespace auction_api.Models
{
    public partial class ItemBid
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public DateTime Time { get; set; }

        public virtual Item Item { get; set; }
        public virtual UserInfo User { get; set; }
    }
}
