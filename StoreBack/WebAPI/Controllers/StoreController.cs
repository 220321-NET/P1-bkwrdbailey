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
        [HttpGet("GetStoreOrderHistory/{storeId}/{sortOrder}")]
        public ActionResult<List<OrderHistory>> GetStoreOrderHistoryAsync(int storeId, int sortOrder)
        {
            return _bl.GetOrderHistoryByStoreAsync(storeId, sortOrder);
        }

        [HttpGet("GetUserOrderHistory/{userId}/{sortOrder}")]
        public ActionResult<List<OrderHistory>> GetUserOrderHistoryAsync(int userId, int sortOrder)
        {
            return _bl.GetOrderHistoryByUserAsync(userId, sortOrder);
        }

        // GET: api/<StoreController>
        [HttpGet("GetInventory/{currStoreId}")]
        public ActionResult<List<Product>> GetStoreInventory(int currStoreId)
        {
            return _bl.GetStoreInventory(currStoreId);
        }

        // POST api/<StoreController>/AddUser
        [HttpPost("AddUser")]
        public ActionResult<User> AddUser([FromBody] User user)
        {
            User newUser = _bl.AddUser(user);
            return Created("api/Store", newUser);
        }

        [HttpPost("AddOrder")]
        public void AddOrder([FromBody] Order order)
        {
            _bl.AddOrder(order);
        }

        [HttpPost("AddProduct/{storeId}")]
        public void AddProduct(int storeId, [FromBody] Product newProduct){
            _bl.AddProduct(storeId, newProduct);
        }

        // PUT api/<StoreController>/UpdateInventory/4
        [HttpPut("UpdateInventory/{storeId}")]
        public void Put(int storeId, [FromBody] Product productToUpdate)
        {
            _bl.UpdateStoreInventory(storeId, productToUpdate);
        }
    }
}
