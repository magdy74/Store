using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Store.Magdy.Core.Services.Contract;
using System.Text;

namespace Store.Magdy.APIs.Attributes
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTime;

        public CachedAttribute(int expireTime)
        {
            _expireTime = expireTime;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cacheKey = GenerateCacheKeyRequest(context.HttpContext.Request);

            var cacheResponse =  await cacheService.GetCacheKeyAsync(cacheKey);

            if (!String.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            var executedContext =  await next();

            if (executedContext.Result is OkObjectResult response) 
            {
                await cacheService.SetCacheKeyAsync(cacheKey, response.Value, TimeSpan.FromSeconds(_expireTime));
            }
        }

        private string GenerateCacheKeyRequest(HttpRequest request) 
        {
            var cacheKey = new StringBuilder();
            cacheKey.Append($"{request.Path}");

            foreach (var (key, value) in request.Query.OrderBy(X => X.Key)) 
            { 
                cacheKey.Append($"|{key}-{value}");
            }

            return cacheKey.ToString();
        }
    }
}
