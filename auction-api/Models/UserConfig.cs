using System;
using System.Collections.Generic;

#nullable disable

namespace auction_api.Models
{
    public partial class UserConfig
    {
        public int UserId { get; set; }
        public decimal? MaxBidPrice { get; set; }
        public decimal? BalanceBidPrice { get; set; }
    }
}
