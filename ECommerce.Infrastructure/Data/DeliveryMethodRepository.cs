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
   public class DeliveryMethodRepository : Repository<DeliveryMethod>, IDeliveryMethodRepository
    {
        private readonly ECommerceContext _db;
        public DeliveryMethodRepository(ECommerceContext db) : base(db)
        {
            _db = db;
        }
    }
}
