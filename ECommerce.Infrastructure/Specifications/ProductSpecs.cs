using ECommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Specifications
{
  public  class ProductSpecs : BaseSpecification<Product>
    {
        public ProductSpecs(ProductSpecParams productParams)
        {
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);
        }
    }
}
