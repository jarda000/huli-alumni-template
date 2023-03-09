using Newtonsoft.Json;

namespace WeatherApp.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, ex.Message);

                // Write the exception to a log file
                string logFilePath = "./ErrorLog.txt";
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Message);
                }

                // Return an appropriate response to the client
                var response = new
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                };

                var json = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
