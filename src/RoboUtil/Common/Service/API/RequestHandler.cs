//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;

//namespace Core.Service.API.Controllers
//{
//    public class RequestHandler
//    {
//        private readonly RequestDelegate _next;
//        //private readonly ILogger _logger;

//        public RequestHandler(RequestDelegate next)
//        {
//            _next = next;
//            //_logger = loggerFactory.CreateLogger<RequestHandler>();
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            var sw = new Stopwatch();
//            sw.Start();
//            await _next(context);
//            sw.Stop();

//            Console.WriteLine(sw.ElapsedMilliseconds.ToString());

//            var isHtml = context.Response.ContentType?.ToLower().Contains("text/html");
//            if (context.Response.StatusCode == 200 && isHtml.GetValueOrDefault())
//            {
//                context.Response.Headers.Add("X-ElapsedTime", new[] { sw.ElapsedMilliseconds.ToString() });
//            }

//        }
//    }

//    public static class RequestFilterExtensions
//    {
//        public static IApplicationBuilder UseRequestHandler(this IApplicationBuilder builder)
//        {
//            return builder.UseMiddleware<RequestHandler>();
//        }
//    }
//}
