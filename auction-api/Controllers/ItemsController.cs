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
        public ActionResult<ItemResponse> GetItems([FromQuery] int pageNo, int pageSize) {
            return _itemService.GetItems(pageNo, pageSize);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Item> GetItemsById(int id)
        {
            return _itemService.GetItemsById(id);
        }
    }
}
