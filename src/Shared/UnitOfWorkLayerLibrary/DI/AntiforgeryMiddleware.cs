//using Microsoft.AspNetCore.Antiforgery;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace UnitOfWork.DI
//{
//    public class AntiforgeryMiddleware : IMiddleware
//    {
//        private readonly IAntiforgery _antiforgery;

//        public AntiforgeryMiddleware(IAntiforgery antiforgery)
//        {
//            _antiforgery = antiforgery;
//        }

//        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
//        {
//            var isGetRequest = string.Equals("GET", context.Request.Method, StringComparison.OrdinalIgnoreCase);
//            if (!isGetRequest)
//            {
//                await _antiforgery.ValidateRequestAsync(context);
//            }

//            await next(context);
//        }
//    }
//}
