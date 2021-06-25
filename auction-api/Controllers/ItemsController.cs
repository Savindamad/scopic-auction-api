using auction_api.IServices;
using auction_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auction_api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public ActionResult<ItemResponse> GetItems([FromQuery] int pageNo, int pageSize, string searchText) {
            return _itemService.GetItems(pageNo, pageSize, searchText);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Item> GetItemsById(int id)
        {
            return _itemService.GetItemsById(id);
        }

        [HttpGet]
        [Route("{id}/user-item")]
        public ActionResult<UserItem> GetItemsUserConfig([FromQuery] int userId, [FromRoute] int id)
        {
            return _itemService.GetItemsUserConfig(userId, id);
        }

        [HttpPut]
        [Route("user-item")]
        public ActionResult<UserItem> UpdateUserItem([FromBody] UserItem userItem)
        {
            return Ok(_itemService.AddOrUpdateUserItem(userItem));
        }

        [HttpGet]
        [Route("{id}/max-bid")]
        public ActionResult<UserItemHistory> GetMaxBidItem([FromRoute] int id)
        {
            return Ok(_itemService.GetMaxBidItem(id));
        }

        [HttpPost]
        [Route("bid-item")]
        public ActionResult<UserItemHistory> AddBidItem([FromBody] UserItemHistory userItem)
        {
            return Ok(_itemService.AddBidItem(userItem));
        }
    }
}
