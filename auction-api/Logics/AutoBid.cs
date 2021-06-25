
using auction_api.Models;
using System.Collections.Generic;

namespace auction_api.Logics
{
    public class AutoBid
    {

        public void ChangeAutoBidFlag(int itemId, int userId)
        {
            var maxBid = new ItemBid(); // todo
            var autoBidUsers = new List<UserItem>(); // same item id
            var userConfig = new List<UserConfig>(); // same item id

            // loop  userConfig
                
        }
    }
}
