using ECommerce.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data
{
   public class UnitOfWork : IUnitOfWork
    {
        private readonly ECommerceContext _db;
        public UnitOfWork(ECommerceContext db)
        {
            _db = db;
            Product = new ProductRepository(_db);
            Category = new CategoryRepository(_db);
            Brand = new BrandRepository(_db);
            DeliveryMethod = new DeliveryMethodRepository(_db);
            Order = new OrderRepository(_db);
        }
        public IProductRepository Product { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IBrandRepository Brand { get; private set; }
        public IDeliveryMethodRepository DeliveryMethod { get; private set; }
        public IOrderRepository Order { get; private set; }
        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task<int> Save()
        {
          return await _db.SaveChangesAsync();
        }
    }
}
