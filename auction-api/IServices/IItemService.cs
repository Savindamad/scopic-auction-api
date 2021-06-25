using auction_api.Models;
using System.Collections.Generic;

namespace auction_api.IServices
{
    public interface IItemService
    {
        ItemResponse GetItems(int pageNo, int pageSize);
        Item GetItemsById(int id);
        UserItem GetItemsUserConfig(int userId,int itemId);
        UserItem AddItemsUserConfig(UserItem userItem);
        UserItem UpdateItemsUserConfig(UserItem userItem);
        ItemBid GetMaxBidItem(int itemId);
        ItemBid AddBidItem(ItemBid userItem);
    }
}
