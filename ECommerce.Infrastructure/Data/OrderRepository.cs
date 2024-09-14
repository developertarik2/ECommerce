using ECommerce.Infrastructure.Data.Interfaces;
using ECommerce.Models.Entities;
using ECommerce.Models.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data
{
   public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ECommerceContext _db;
        public OrderRepository(ECommerceContext db) : base(db)
        {
            _db = db;
        }
    }
}
