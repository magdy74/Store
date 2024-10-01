using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using Store.Magdy.Core.Entities;
using Store.Magdy.Repository.Data.Contexts;


namespace Store.Magdy.Repository.Data
{
    public static class StoreDbContextSeed
    {
        public async static Task SeedAsync(StoreDbContext _context)
        {

            if (_context.Brands.Count() == 0)
            {
                // Brand
                // 1. Read Data From Json File

                var brandsData = File.ReadAllText(@"../Store.Magdy.Repository/Data/DataSeed/brands.json");

                // 2. Convert Json String To List<T>

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                // 3. Seed Data To DB

                if (brands is not null && brands.Count() > 0)
                {
                    await _context.Brands.AddRangeAsync(brands);

                }
            }


            if (_context.Types.Count() == 0)
            {
                // Brand
                // 1. Read Data From Json File

                var typesData = File.ReadAllText(@"../Store.Magdy.Repository/Data/DataSeed/types.json");

                // 2. Convert Json String To List<T>

                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                // 3. Seed Data To DB

                if (types is not null && types.Count() > 0)
                {
                    await _context.Types.AddRangeAsync(types);

                }
            }

            if (_context.Products.Count() == 0)
            {
                // Brand
                // 1. Read Data From Json File

                var productsData = File.ReadAllText(@"../Store.Magdy.Repository/Data/DataSeed/products.json");

                // 2. Convert Json String To List<T>

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                // 3. Seed Data To DB

                if (products is not null && products.Count() > 0)
                {
                    await _context.Products.AddRangeAsync(products);

                }
            }

            await _context.SaveChangesAsync();
        }


    }
}
