using ECommerce.Models.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Interfaces
{
  public interface IDeliveryMethodRepository : IRepository<DeliveryMethod>
    {
    }
}
