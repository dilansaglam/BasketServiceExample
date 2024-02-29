using BasketServiceExample.Models;
using BasketServiceExample.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketServiceExample.Services
{
    public class BasketService
    {
        private readonly IBasketRepository _basketRepository;
        public BasketService(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<Basket> GetBasketDetail(int userId)
        {
            return await _basketRepository.GetByUserIdAsync(userId);
        }

        public async Task<bool> AddProduct(int userId, Product product)
        {
            return await _basketRepository.InsertAsync(userId, product);
        }

        public async Task<bool> RemoveProduct(int userId, Product product)
        {
            return await _basketRepository.DeleteAsync(userId, product);
        }
    }
}
