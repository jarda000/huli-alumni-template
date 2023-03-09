using Newtonsoft.Json;

namespace WeatherApp.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                string logFilePath = "./ErrorLog.txt";
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Message);
                }
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
