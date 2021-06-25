using auction_api.IServices;
using auction_api.Models;
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
            itemResponse.Items = _dbContext.Items.Skip((pageNo-1) * pageSize).Take(pageSize).ToList();
            return itemResponse;
        }

        public Item GetItemsById(int id)
        {
            var item = _dbContext.Items.FirstOrDefault(x => x.Id == id);
            return item;
        }
    }
}
