using WeatherApp.Interfaces;

namespace WeatherApp.Middleware
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ITokenService tokenService)
        {
            var requestToken = httpContext.Request.Headers["Authorization"];

            int userId = tokenService.GetUserFromJwtClaim(requestToken);

            if (userId != -1)
            {
                httpContext.Items["Id"] = userId;
            }

            await next(httpContext);
        }
    }
}
