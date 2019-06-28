using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeDev.Core.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("2.0")]
    [Route("api/v{v:apiVersion}/values")]
    [ApiController]
    public class ValuesV2Controller : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {

            return new string[] { "v2:value1", "v2:value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var user = HttpContext.User.Identity.Name;

            return $"Version 2.0: value:{id} - user: {user}";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        { }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        { }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        { }
    }
}