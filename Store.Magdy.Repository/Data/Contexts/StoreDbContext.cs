﻿using Microsoft.EntityFrameworkCore;
using Store.Magdy.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Store.Magdy.Repository.Data.Contexts
{
    // CLR
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> Brands { get; set; }
        public DbSet<ProductType> Types { get; set; }

    }
}