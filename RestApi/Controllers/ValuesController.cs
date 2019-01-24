using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sisc.RestApi.Models;

namespace Sisc.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        
        private List<Value> AllValues;
        public ValuesController()
        {
            AllValues = new List<Value>();
            for (var i = 0; i < 100; i++)
            {
                AllValues.Add(new Value { Id = i , Name = $"Name {i}"});
            }

        }
        // GET api/values
        [HttpGet]
        public ActionResult<List<Value>> Get()
        {
            return AllValues;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Value> Get(int id)
        {
            return AllValues.FirstOrDefault(x=>x.Id == id);
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
