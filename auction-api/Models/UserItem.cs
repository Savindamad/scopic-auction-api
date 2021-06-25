using System.Text.Json.Serialization;

#nullable disable

namespace auction_api.Models
{
    public partial class UserItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public bool IsAutoBid { get; set; }

        [JsonIgnore]
        public virtual Item Item { get; set; }
        [JsonIgnore]
        public virtual UserInfo User { get; set; }
    }
}
