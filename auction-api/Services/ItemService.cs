using auction_api.IServices;
using auction_api.Models;
using System;
using System.Linq;

namespace auction_api.Services
{
    public class ItemService : IItemService
    {
        AuctionDbContext _dbContext;
        public ItemService(AuctionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ItemResponse GetItems(int pageNo, int pageSize)
        {
            var itemResponse = new ItemResponse();
            itemResponse.Count = _dbContext.Items.Count();
            itemResponse.Items = _dbContext.Items.Where(x => (DateTime.Compare(x.ClosingTime, DateTime.Now)> 0)).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            return itemResponse;
        }

        public Item GetItemsById(int id)
        {
            var item = _dbContext.Items.FirstOrDefault(x => x.Id == id);
            return item;
        }

        public UserItem GetItemsUserConfig(int userId, int itemId)
        {
            var userItem = _dbContext.UserItems.FirstOrDefault(x => x.UserId == userId && x.ItemId == itemId);
            return userItem;
        }

        public UserItem AddItemsUserConfig(UserItem userItem)
        {
            try
            {
                _dbContext.Add<UserItem>(userItem);
                _dbContext.SaveChanges();
                return userItem;
            }
            catch
            {
                throw;
            }
        }
        public UserItem UpdateItemsUserConfig(UserItem userItem)
        {
            try
            {
                _dbContext.Update<UserItem>(userItem);
                _dbContext.SaveChanges();
                return userItem;
            }
            catch
            {
                throw;
            }
        }

        public ItemBid GetMaxBidItem(int itemId)
        {
            var item = _dbContext.ItemBids.Where(x=> x.ItemId == itemId).OrderByDescending(x => x.Price).FirstOrDefault();
            return item;
        }

        public ItemBid AddBidItem(ItemBid item)
        {
            try
            {
                _dbContext.Add(item);
                _dbContext.SaveChanges();
                return item;
            }
            catch
            {
                throw;
            }

        }
    }
}
