using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace auction_api.Models
{
    public partial class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string Role { get; set; }
        [JsonIgnore]
        public virtual ICollection<ItemBid> ItemBids { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserItem> UserItems { get; set; }
    }
}
