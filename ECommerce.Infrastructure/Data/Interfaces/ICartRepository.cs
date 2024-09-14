using ECommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Interfaces
{
   public interface ICartRepository
    {
        Task<CustomerCart> GetCartAsync(string basketId);
     //   Task<CustomerCart> UpdateCartOnly(CustomerCart basket);
        Task<CustomerCart> UpdateCartAsync(CustomerCart basket);
        Task<bool> DeleteCartAsync(string basketId);
    }
}
