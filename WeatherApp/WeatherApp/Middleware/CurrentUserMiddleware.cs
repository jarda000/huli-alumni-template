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

            int playerId = tokenService.GetUserFromJwtClaim(requestToken);

            if (playerId != -1)
            {
                httpContext.Items["Id"] = playerId;
            }

            await next(httpContext);
        }
    }
}
