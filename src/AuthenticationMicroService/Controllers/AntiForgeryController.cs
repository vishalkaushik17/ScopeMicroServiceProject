//using Microsoft.AspNetCore.Antiforgery;
//using Microsoft.AspNetCore.Mvc;
//using SharedLibrary.Services.CustomFilters;
//namespace AuthenticateService.Controllers
//{
//    //[Route("api/xsrf-token/[controller]")]
//    [ApiVersion("1.0")]
//    [Route("api/v{version:apiVersion}/[controller]")]
//    [ApiController]
//    public class AntiForgeryController : Controller
//    {
//        private IAntiforgery _antiforgery;

//        public AntiForgeryController(IAntiforgery antiforgery)
//        {
//            _antiforgery = antiforgery;
//        }

//        [AllowAnonymous]
//        [HttpGet]
//        [Route("test")]
//        public string test()
//        {
//            return "success!";
//        }


//        [IgnoreAntiforgeryToken]
//        [AllowAnonymous]
//        [HttpGet]
//        [Route("xsrf")]
//        public IActionResult Get()
//        {
//            // Creates and sets the cookie token in a cookie
//            // Cookie name will be like ".AspNetCore.Antiforgery.pG4SaGh5yDI"
//            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);

//            // Take request token (which is different from a cookie token)
//            var headerToken = tokens.RequestToken;
//            // Set another cookie for a request token
//            Response.Cookies.Append("XSRF-TOKEN", headerToken, new CookieOptions
//            {
//                HttpOnly = false
//            });
//            return NoContent();
//        }
//    }
//}
