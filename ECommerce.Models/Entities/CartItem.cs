using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Entities
{
   public class CartItem
    {
        [Key]
      //  [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Cart_Item_Id { get; set; }
        public int Id { get; set; }
        public string ProductName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public virtual CustomerCart CustomerCart { get; set; }
    }
}
