using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace HomeDev.Core.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/values")]
    [Route("api/v{v:apiVersion}/values")]
    [ApiController]
    public class ValuesV1Controller : ControllerBase
    {
        private readonly ILogger _logger;

        public ValuesV1Controller(
            ILogger logger
        )
        {
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _logger.Information("Testing logging data to Serilog");
            return new string[] { "v1:value1", "v1:value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var user = HttpContext.User.Identity.Name;

            _logger.Warning("What is going on, where is my logging?");

            return $"Version 1.0:  value:{id} - user: {user}";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
