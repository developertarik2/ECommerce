using ECommerce.Infrastructure.Data.Interfaces;
using ECommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data
{
   public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ECommerceContext _db;
        public ProductRepository(ECommerceContext db) : base(db)
        {
            _db = db;
        }
    }
}
