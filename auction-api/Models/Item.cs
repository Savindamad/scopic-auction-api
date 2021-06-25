﻿using System;
using System.Collections.Generic;

#nullable disable

namespace auction_api.Models
{
    public partial class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public DateTime ClosingTime { get; set; }

        public virtual ICollection<ItemBid> ItemBids { get; set; }
    }
}
