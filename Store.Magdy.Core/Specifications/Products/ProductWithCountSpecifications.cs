using Store.Magdy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Magdy.Core.Specifications.Products
{
    public class ProductWithCountSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithCountSpecifications(ProductSpecParams productSpec) : base
    (
        P =>
        (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search))
        &&
        (!productSpec.brandId.HasValue || P.BrandId == productSpec.brandId) 
        && 
        (!productSpec.typeId.HasValue || P.TypeId == productSpec.typeId)
    )
        {

        }

    }
}
