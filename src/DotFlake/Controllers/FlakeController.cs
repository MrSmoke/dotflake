namespace DotFlake.Controllers
{
    using Generators;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc;
    using Timing;

    public class FlakeController : Controller
    {
        private static readonly IIdGenerator<long> Generator = new FlakeGenerator(new StopwatchTimeSource(new SystemClock(), new StopwatchTimeSourceOptions()));

        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok(new IdReponse<long>(Generator.Next()));
        }
    }
}
