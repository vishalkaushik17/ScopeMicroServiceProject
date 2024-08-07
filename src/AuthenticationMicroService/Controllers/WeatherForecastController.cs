////using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using SharedLibrary.Services;

//namespace AuthenticateService.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]

//    public class WeatherForecastController : ControllerBase
//    {
//        private static readonly string[] Summaries = new[]
//        {
//        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//    };

//        private readonly ILogger<WeatherForecastController> _logger;


//        public WeatherForecastController(ILogger<WeatherForecastController> logger)
//        {
//            _logger = logger;


//        }

//        [HttpGet(Name = "GetWeatherForecast")]
//        [AllowAnonymous]
//        public async Task<string> Get()
//        {



//            throw new Exception("my exceptlsdfj sdlfkjf jsdkf dlfdslf slfjsdjf slfjksdf ldskjfdsl flskdjf dslfkjdsf ldsjflds lsdkjfdls fjsdd slfkdjsl fjfldskjslfion");
//            return "hello";
//        }

//    }
//}