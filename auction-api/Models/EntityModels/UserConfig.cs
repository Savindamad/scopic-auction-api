using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace auction_api.Models
{
    public partial class UserConfig
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal? MaxBidPrice { get; set; }
        public decimal? BalanceBidPrice { get; set; }
        [JsonIgnore]
        public virtual UserInfo User { get; set; }
    }
}
