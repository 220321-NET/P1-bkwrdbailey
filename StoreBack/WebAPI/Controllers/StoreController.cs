using Microsoft.AspNetCore.Mvc;
using Models;
using BL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreBL _bl;
        // Maybe Try Caching?????????
        public StoreController(IStoreBL bl) 
        {
            _bl = bl;
        }

        // GET: api/<StoreController>
        [HttpGet("GetUsers")]
        public async Task<List<User>> GetUsersAsync()
        {
            return await _bl.GetAllUsersAsync();
        }

        // GET: api/<StoreController>
        [HttpGet("GetStores")]
        public async Task<List<Store>> GetStoresAsync()
        {
            return await _bl.GetAllStoresAsync();
        }

        // GET api/<StoreController>/5
        [HttpGet]
        [Route("GetStoreOrderHistory")]
        public async Task<List<OrderHistory>> GetStoreOrderHistoryAsync(int storeId)
        {
            return await _bl.GetOrderHistoryByStoreAsync(storeId);
        }

        [HttpGet]
        [Route("GetUserOrderHistory")]
        public async Task<List<OrderHistory>> GetUserOrderHistoryAsync(int userId)
        {
            return await _bl.GetOrderHistoryByUserAsync(userId);
        }

        // GET: api/<StoreController>
        [HttpGet("GetInventory")]
        public ActionResult<List<Product>> GetStoreInventory(int currStoreId)
        {
            return _bl.GetStoreInventory(currStoreId);
        }

        // POST api/<StoreController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<StoreController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StoreController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
