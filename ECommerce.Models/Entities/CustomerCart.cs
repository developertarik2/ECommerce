using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Entities
{
  public  class CustomerCart
    {
        public CustomerCart()
        {

        }
        public CustomerCart(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public int? DeliveryMethodId { get; set; }
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingPrice { get; set; }
    }
}
