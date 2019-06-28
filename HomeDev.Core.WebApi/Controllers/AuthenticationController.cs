using HomeDev.Core.WebApi.Interfaces;
using HomeDev.Core.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeDev.Core.WebApi.Controllers
{
    
    [Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/authentication")]
    [Route("api/v{v:apiVersion}/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController (
            IAuthenticationService authService
        )
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost, Route("token")]
        public ActionResult Token ([FromBody] AuthTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest ("Invalid Request");
            }

            if (_authService.IsAuthenticated (request, out string token))
            {
                return Ok (token);
            }
            else
            {
                return Unauthorized ();
            }

        }
    }
}