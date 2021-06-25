using System;
using System.Collections.Generic;

#nullable disable

namespace auction_api.Models
{
    public partial class UserItem
    {
        public UserItem()
        {
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public bool IsAutoBid { get; set; }
        public decimal? UserMaxBid { get; set; }
        public int? MaxBidUserItemHistoryId { get; set; }

        public virtual Item Item { get; set; }
        public virtual UserItemHistory MaxBidUserItemHistory { get; set; }
        public virtual UserInfo User { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
