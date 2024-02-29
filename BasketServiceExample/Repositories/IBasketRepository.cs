using BasketServiceExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketServiceExample.Repositories
{
    public interface IBasketRepository
    {
        Task<Basket> GetByUserIdAsync(int userId);
        Task<bool> InsertAsync(int userId, Product product);
        Task<bool> DeleteAsync(int userId, Product product);
    }
}
