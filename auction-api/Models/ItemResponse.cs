using System.Collections.Generic;

namespace auction_api.Models
{
    public class ItemResponse
    {
        public List<Item> Items { get; set; }
        public int Count { get; set; }
    }
}
