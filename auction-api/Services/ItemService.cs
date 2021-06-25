using auction_api.IServices;
using auction_api.Models;
using System;
using System.Collections.Generic;
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

        public ItemResponse GetItems(int pageNo, int pageSize, string searchText)
        {
            var itemResponse = new ItemResponse();
            if (searchText == null)
            {
                itemResponse.Count = _dbContext.Items.Where(x => (DateTime.Compare(x.ClosingTime, DateTime.Now) > 0)).Count();
                itemResponse.Items = _dbContext.Items.Where(x => (DateTime.Compare(x.ClosingTime, DateTime.Now) > 0)).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                itemResponse.Count = _dbContext.Items.Where(x => (DateTime.Compare(x.ClosingTime, DateTime.Now) > 0) && (x.Name.Contains(searchText) || x.Description.Contains(searchText))).Count();
                itemResponse.Items = _dbContext.Items.Where(x => (DateTime.Compare(x.ClosingTime, DateTime.Now) > 0) && (x.Name.Contains(searchText) || x.Description.Contains(searchText)))
                    .Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            return itemResponse;
        }

        public Item GetItemsById(int id)
        {
            var item = _dbContext.Items.FirstOrDefault(x => x.Id == id);
            return item;
        }

        public Item UpdateItem(Item item)
        {
            try
            {
                _dbContext.Update<Item>(item);
                _dbContext.SaveChanges();
                return item;
            }
            catch
            {
                throw;
            }
        }

        public UserItem GetItemsUserConfig(int userId, int itemId)
        {
            var userItem = _dbContext.UserItems.FirstOrDefault(x => x.UserId == userId && x.ItemId == itemId);
            return userItem;
        }

        public UserItem AddUserItem(UserItem userItem)
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

        public UserItem UpdateUserItem(UserItem userItem)
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

        public UserItem AddOrUpdateUserItem(UserItem userItem)
        {
            if (userItem.Id == 0)
            {
                userItem = AddUserItem(userItem);
            }
            else
            {
                userItem = UpdateUserItem(userItem);
                if (userItem.IsAutoBid)
                {
                    UpdateAutoBidForAutoBid(userItem);
                }
            }
            return userItem;
        }

        public UserItemHistory GetMaxBidItem(int itemId)
        {
            var item = _dbContext.UserItemHistories.Where(x => x.ItemId == itemId).OrderByDescending(x => x.Price).FirstOrDefault();
            return item;
        }

        public UserItemHistory AddBidItem(UserItemHistory userItemHistory)
        {
            return AddOrUpdateNewBid(userItemHistory, false);
        }

        public UserItemHistory AddOrUpdateNewBid(UserItemHistory userItemHistory, bool stopLoop)
        {
            try
            {
                _dbContext.Add(userItemHistory);
                _dbContext.SaveChanges();

                var item = GetItemsById(userItemHistory.ItemId);
                var userItem = _dbContext.UserItems.Where(x => x.ItemId == userItemHistory.ItemId && x.UserId == userItemHistory.UserId).FirstOrDefault();
                if (userItem == null)
                {
                    var newItem = new UserItem();
                    newItem.ItemId = userItemHistory.ItemId;
                    newItem.UserId = userItemHistory.UserId;
                    newItem.IsAutoBid = false;
                    newItem.UserMaxBid = userItemHistory.Price;
                    newItem.MaxBidUserItemHistoryId = userItemHistory.Id;
                    newItem = AddUserItem(newItem);

                    item.MaxBidUserItemId = newItem.Id;
                }
                else
                {
                    userItem.UserMaxBid = userItemHistory.Price;
                    userItem.MaxBidUserItemHistoryId = userItemHistory.Id;
                    userItem = UpdateUserItem(userItem);

                    item.MaxBidUserItemId = userItem.Id;
                }

                UpdateItem(item);
                if (!stopLoop)
                {
                    UpdateAutoBidForNewBid(userItemHistory);
                }

                return userItemHistory;
            }
            catch
            {
                throw;
            }
        }

        private void UpdateAutoBidForNewBid(UserItemHistory userItemHistory)
        {
            var userItemAutoBid = _dbContext.UserItems.Where(x => x.ItemId == userItemHistory.ItemId && x.IsAutoBid == true).ToList();
            var userBalanceList = new List<UserBalance>();
            userItemAutoBid.ForEach(x =>
            {
                var userMaxAmount = _dbContext.UserConfigs.Where(z => z.UserId == x.UserId).FirstOrDefault().MaxBidPrice;
                var userItemMaxBids = _dbContext.UserItems
                .Where(y => y.UserId == x.UserId && y.ItemId != x.ItemId)
                .Join(_dbContext.Items,
                      p => p.Id,
                      e => e.MaxBidUserItemId,
                      (p, e) => new
                      {
                          Id = p.Id,
                          ItemId = p.ItemId,
                          Amount = p.UserMaxBid
                      }
                      ).ToList();

                var userBalance = new UserBalance();
                userBalance.UserId = x.UserId;
                userBalance.BalanceAmount = (decimal)(userMaxAmount - (decimal)userItemMaxBids.Sum(item => item.Amount));
                userBalanceList.Add(userBalance);
            });

            var maxAmountUser = userBalanceList.OrderByDescending(u => u.BalanceAmount).FirstOrDefault();

            if (maxAmountUser != null && maxAmountUser.BalanceAmount >= userItemHistory.Price + 1)
            {
                var newUserItemHistory = new UserItemHistory();
                newUserItemHistory.ItemId = userItemHistory.ItemId;
                newUserItemHistory.Price = userItemHistory.Price + 1;
                newUserItemHistory.Time = DateTime.Now;
                newUserItemHistory.UserId = maxAmountUser.UserId;

                AddOrUpdateNewBid(newUserItemHistory, true);
            }
        }

        private void UpdateAutoBidForAutoBid(UserItem userItem)
        {
            var userItemAutoBid = _dbContext.UserItems.Where(x => x.ItemId == userItem.ItemId && x.IsAutoBid == true).ToList();
            var userBalanceList = new List<UserBalance>();
            userItemAutoBid.ForEach(x =>
            {
                var userMaxAmount = _dbContext.UserConfigs.Where(z => z.UserId == x.UserId).FirstOrDefault().MaxBidPrice;
                var userItemMaxBids = _dbContext.UserItems
                .Where(y => y.UserId == x.UserId && y.ItemId != x.ItemId)
                .Join(_dbContext.Items,
                      p => p.Id,
                      e => e.MaxBidUserItemId,
                      (p, e) => new
                      {
                          Id = p.Id,
                          ItemId = p.ItemId,
                          Amount = p.UserMaxBid
                      }
                      ).ToList();

                var userBalance = new UserBalance();
                userBalance.UserId = x.UserId;
                userBalance.BalanceAmount = (decimal)(userMaxAmount - (decimal)userItemMaxBids.Sum(item => item.Amount));
                userBalanceList.Add(userBalance);
            });

            var maxAmountUser = userBalanceList.OrderByDescending(u => u.BalanceAmount).FirstOrDefault();
            var newUserItemHistory = new UserItemHistory();
            if (maxAmountUser != null && maxAmountUser.UserId == userItem.UserId)
            {
                var secondMaxAmountUser = userBalanceList.OrderByDescending(u => u.BalanceAmount).Skip(1).FirstOrDefault();

                if (secondMaxAmountUser != null)
                {
                    newUserItemHistory.Price = secondMaxAmountUser.BalanceAmount + 1;
                }
                else
                {
                    var maxBidItem = GetMaxBidItem(userItem.ItemId);
                    newUserItemHistory.Price = maxBidItem.Price + 1;
                }
                newUserItemHistory.ItemId = userItem.ItemId;
                newUserItemHistory.Time = DateTime.Now;
                newUserItemHistory.UserId = maxAmountUser.UserId;

                AddOrUpdateNewBid(newUserItemHistory, true);
            }
        }
    }
}
