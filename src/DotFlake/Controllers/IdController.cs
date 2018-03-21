namespace DotFlake.Controllers
{
    using System.Net;
    using Exceptions;
    using Microsoft.AspNetCore.Mvc;
    using Responses;

    public class IdController : Controller
    {
        private readonly IIdGeneratorFactory _idGeneratorFactory;

        public IdController(IIdGeneratorFactory idGeneratorFactory)
        {
            _idGeneratorFactory = idGeneratorFactory;
        }

        [HttpGet("{GeneratorName}")]
        public IActionResult Get(string generatorName)
        {
            try
            {
                var id = _idGeneratorFactory.Next(generatorName);

                return Ok(new IdReponse<object>(id));
            }
            catch (UnknownGeneratorException e)
            {
                return Error(HttpStatusCode.NotFound, e.Message);
            }
            catch (DotFlakeException e)
            {
                return Error(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        private IActionResult Error(HttpStatusCode code, string errorMessage)
        {
            return StatusCode((int)code, new ErrorResponse(errorMessage));
        }
    }
}
