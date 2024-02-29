using BasketServiceExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketServiceExample.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        public BasketRepository() { }

        public async Task<Basket> GetByUserIdAsync(int userId)
        {
            // filter db by user id
            await Task.Delay(100); // to simulate the behavior of an async db function
            return new Basket(); 
        }

        public async Task<bool> InsertAsync(int userId, Product product) 
        {
            // filter by user id and insert product
            await Task.Delay(100); // to simulate the behavior of an async db function
            return true;
        }

        public async Task<bool> DeleteAsync(int userId, Product product)
        {
            // filter by user id and delete product from products list
            await Task.Delay(100); // to simulate the behavior of an async db function
            return true;
        }


    }
}
