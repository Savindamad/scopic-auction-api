using auction_api.Models;

namespace auction_api.IServices
{
    public interface IItemService
    {
        ItemResponse GetItems(int pageNo, int pageSize, string searchText);
        Item GetItemsById(int id);
        UserItem GetItemsUserConfig(int userId,int itemId);
        UserItem AddOrUpdateUserItem(UserItem userItem);
        UserItemHistory GetMaxBidItem(int itemId);
        UserItemHistory AddBidItem(UserItemHistory userItem);
        UserItemHistory AddOrUpdateNewBid(UserItemHistory userItemHistory, bool stopLoop);
    }
}
