using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Magdy.Core.Dtos.Products;
using Store.Magdy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Store.Magdy.Core.Mapping.Products
{
    public class ProductProfile : Profile
    {
        private readonly IConfiguration _configuration;

        //public ProductProfile(IConfiguration configuration)
        //{
        //    _configuration = configuration;

        //    CreateMap<Product, ProductDto>()
        //        .ForMember(d => d.BrandName,options => options.MapFrom(s => s.Brand.Name))
        //        .ForMember(d => d.TypeName,options => options.MapFrom(s => s.Type.Name))
        //        .ForMember(d => d.PictureUrl, options => options.MapFrom(s => $"{configuration["BASEURL"]}{s.PictureUrl}"))
        //        ;
        //    CreateMap<ProductBrand, TypeBrandDto>();
        //    CreateMap<ProductType, TypeBrandDto>();
        //}

        public ProductProfile(IConfiguration configuration)
        {
            _configuration = configuration;
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.productBrand, options => options.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.productType, options => options.MapFrom(s => s.Type.Name))
                .ForMember(d => d.PictureUrl, options => options.MapFrom(new PictureUrlResolver(_configuration)))
                ;
            CreateMap<ProductBrand, TypeBrandDto>();
            CreateMap<ProductType, TypeBrandDto>();
        }
    }
}
