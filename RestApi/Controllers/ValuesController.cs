using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Newtonsoft.Json.Serialization;
using Sisc.RestApi.Models;
using Sisc.RestApi.Services;

namespace Sisc.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        
        private readonly List<Value> AllValues;
        public ValuesController(IMockCreator  mockCreator)
        {
            AllValues = mockCreator.AllValues;

        }
        // GET api/values/GetAll
        [HttpGet]
        [Route("GetAll")]
        public ActionResult<List<Value>> GetAll()
        {
            return AllValues;
        }
        // GET api/values/GetSome?amount=3
        [HttpGet]
        [Route("GetSome")]
        public ActionResult<List<Value>> GetSome([FromQuery] int amount)
        {
            return AllValues.GetRange(0, amount);
        }

        // GET api/values/Get/5
        [HttpGet("{id}")]
        [Route("Get/{id}")]
        public ActionResult<Value> Get(int id)
        {
            return AllValues.FirstOrDefault(x=>x.Id == id);
        }

        // POST api/values/add
        [HttpPost]
        [Route("Add")]
        public ActionResult Add([FromBody] Value newValue)
        {
            try
            {
                var newId = 1;
                if (AllValues.Count > 0)
                {
                    newId = AllValues.Max(x => x.Id);
                }
                newValue.Id = newId+1;
                AllValues.Add(newValue);
                
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        // PUT api/values/Update
        [HttpPut]
        [Route("Update")]
        public ActionResult Update( [FromBody] Value updateValue)
        {
            var found = AllValues.FirstOrDefault(x => x.Id == updateValue.Id);
            if (found != null)
            {
                found.Name = updateValue.Name;
                return Ok();
            }
            return NotFound();
        }

        // DELETE api/values/Delete/5
        [HttpDelete("{id}")]
        [Route("Delete")]
        public ActionResult Delete(int id)
        {
            var found = AllValues.FirstOrDefault(x => x.Id == id);
            if (found != null)
            {
                AllValues.RemoveAt(AllValues.IndexOf(found));
                return Ok();
            }
            return NotFound();
        }

    }
}
