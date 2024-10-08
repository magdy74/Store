using Store.Magdy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Magdy.Core.Specifications.Products
{
    public class ProductSpecification : BaseSpecifications<Product, int>
    {

        public ProductSpecification(ProductSpecParams productSpec) : base
            (
                P =>
                (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search))
                &&
                (!productSpec.brandId.HasValue || P.BrandId == productSpec.brandId) 
                && (!productSpec.typeId.HasValue || P.TypeId == productSpec.typeId)
            )
        {
            AddIncludes();

            // Name, PriceAsc, PriceDesc
            

            if (!string.IsNullOrEmpty(productSpec.sort))
            {
                var result = productSpec.sort.ToLower();
                switch(result) 
                {
                    case "priceasc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else{
                AddOrderBy(P => P.Name);
            }

            ApplyPagination(productSpec.pageSize * (productSpec.pageIndex - 1), productSpec.pageSize);
        }

        public ProductSpecification(int id)
        {
            Criteria = P => P.Id == id;
            AddIncludes();
        }

        private void AddIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Type);
        }

    }
}
