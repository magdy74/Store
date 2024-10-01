using AutoMapper;
using Store.Magdy.Core.Dtos.Products;
using Store.Magdy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Magdy.Core.Mapping.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.BrandName,options => options.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.TypeName,options => options.MapFrom(s => s.Type.Name))
                ;
            CreateMap<ProductBrand, TypeBrandDto>();
            CreateMap<ProductType, TypeBrandDto>();
        }
    }
}
