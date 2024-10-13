
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Magdy.APIs.Errors;
using Store.Magdy.APIs.Middlewares;
using Store.Magdy.Core;
using Store.Magdy.Core.Mapping.Products;
using Store.Magdy.Core.Services.Contract;
using Store.Magdy.Repository;
using Store.Magdy.Repository.Data;
using Store.Magdy.Repository.Data.Contexts;
using Store.Magdy.Service.Services.Products;
using Store.Magdy.APIs.Helper;
namespace Store.Magdy.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDependency(builder.Configuration);

            var app = builder.Build();

            await app.ConfigureMiddlewareAsync();

            app.Run();
        }
    }
}
