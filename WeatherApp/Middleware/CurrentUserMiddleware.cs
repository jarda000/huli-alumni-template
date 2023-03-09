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
            // Retrieve the JWT token from the Authorization header
            var requestToken = httpContext.Request.Headers["Authorization"];

            // Use the token service to get the user ID from the JWT token
            int userId = tokenService.GetUserFromJwtClaim(requestToken);

            // If the user ID is not -1 (meaning it was successfully retrieved from the token), 
            // add it to the Items dictionary of the HttpContext, so it can be accessed later in the request pipeline
            if (userId != -1)
            {
                httpContext.Items["Id"] = userId;
            }

            // Call the next middleware in the pipeline
            await next(httpContext);
        }
    }
}
