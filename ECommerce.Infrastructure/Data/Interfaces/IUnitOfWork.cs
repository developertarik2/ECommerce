using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Interfaces
{
  public  interface IUnitOfWork : IDisposable
    {
        IProductRepository Product { get; }
        IBrandRepository Brand { get; }
        ICategoryRepository Category { get; }
        IDeliveryMethodRepository DeliveryMethod { get; }
        IOrderRepository Order { get; }
        Task<int> Save();
    }
}
