﻿using AutoMapper;
using Store.Magdy.Core;
using Store.Magdy.Core.Dtos.Products;
using Store.Magdy.Core.Entities;
using Store.Magdy.Core.Helper;
using Store.Magdy.Core.Services.Contract;
using Store.Magdy.Core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Magdy.Service.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductSpecParams productSpec)
        {
            var spec = new ProductSpecification(productSpec);

            var mappedProducts = _mapper.Map<IEnumerable<ProductDto>>(await _unitOfWork.Repository<Product, int>().GetAllWithSpecsAsync(spec));

            var countSpec = new ProductWithCountSpecifications(productSpec);

            var count = await _unitOfWork.Repository<Product, int>().GetCountAsync(countSpec);

            return new PaginationResponse<ProductDto>(productSpec.pageSize, productSpec.pageIndex, count, mappedProducts);
        }


        public async Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync()
        {
            return _mapper.Map<IEnumerable<TypeBrandDto>>(await _unitOfWork.Repository<ProductType, int>().GetAllAsync());
        }


        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var spec = new ProductSpecification(id);
            var product = await _unitOfWork.Repository<Product, int>().GetWithSpecsAsync(spec);
            var mappedProduct = _mapper.Map<ProductDto>(product);
            return mappedProduct;
        }


        public async Task<IEnumerable<TypeBrandDto>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand, int>().GetAllAsync();
            var mappedBrands = _mapper.Map<IEnumerable<TypeBrandDto>>(brands);
            return mappedBrands;
        }


    }
}
