using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace auction_api.Models
{
    public partial class UserItemHistory
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public DateTime Time { get; set; }
        [JsonIgnore]
        public virtual Item Item { get; set; }
        [JsonIgnore]
        public virtual UserInfo User { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserItem> UserItems { get; set; }
    }
}
