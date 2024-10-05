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

        public ProductSpecification(string? sort, int? brandId, int? typeId) : base
            (
                P => (!brandId.HasValue || P.BrandId == brandId) && (!typeId.HasValue || P.TypeId == typeId)
            )
        {
            AddIncludes();

            // Name, PriceAsc, PriceDesc
            

            if (!string.IsNullOrEmpty(sort))
            {
                var result = sort.ToLower();
                switch(result) 
                {
                    case "name":
                        OrderBy = P => P.Name;
                        break;
                    case "priceasc":
                        OrderBy = P => P.Price;
                        break;
                    case "pricedesc":
                        OrderByDescending = P => P.Price;
                        break;
                    default:
                        OrderBy = P => P.Name;
                        break;
                }
            }
            else{
                OrderBy = P => P.Name;
            }


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
