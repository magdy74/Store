using Store.Magdy.APIs.Middlewares;
using Store.Magdy.Repository.Data.Contexts;
using Store.Magdy.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Store.Magdy.Repository.Identity.Contexts;
using Store.Magdy.Repository.Identity.DataSeed;
using Microsoft.AspNetCore.Identity;
using Store.Magdy.Core.Entities.Identity;

namespace Store.Magdy.APIs.Helper
{
    public static class ConfigureMiddleware
    {

        public static async Task<WebApplication> ConfigureMiddlewareAsync(this WebApplication app) 
        {
            app.UseStaticFiles();

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<StoreDbContext>();
            var identityContext = services.GetRequiredService<StoreIdentityDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();


            try
            {
                await context.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(context);

                await identityContext.Database.MigrateAsync();
                await StoreIdentityDbContextSeed.SeedAppUserAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "there are problems during apply migrations !");
            }

            app.UseMiddleware<ExceptionMiddleware>(); // Configure user defined middleware

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
 
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseHttpsRedirection();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

    }
}
