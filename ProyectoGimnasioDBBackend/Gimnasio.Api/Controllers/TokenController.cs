using Gimnasio.Core.CustomEntities;
using Gimnasio.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gimnasio.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Authentication()
        {

            return Ok();
        }

        // private string GenerateToken()
        // {
        //     string secretKey = _configuration["Authentication:SecretKey"];
        //     return "";
        // }
    }
}