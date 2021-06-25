using auction_api.Models;
using System.Collections.Generic;

namespace auction_api.IServices
{
    public interface IItemService
    {
        ItemResponse GetItems(int pageNo, int pageSize);
        Item GetItemsById(int id);
        //Employee AddEmployee(Employee employee);
        //Employee UpdateEmployee(Employee employee);
        //Employee DeleteEmployee(int id);
    }
}
