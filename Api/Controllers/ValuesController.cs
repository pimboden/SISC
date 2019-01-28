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
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        
        private readonly List<Value> AllValues;
        public ValuesController(IMockCreator  mockCreator)
        {
            AllValues = mockCreator.AllValues;

        }
        
        /// <summary>
        /// Gets a Paged amount of values 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/values/GetAll
        ///or
        /// 
        ///     GET api/values/GetAll?page=1&amp;pageSize=1000
        ///
        /// </remarks>
        /// <param name="page">Current paging index</param>
        /// <param name="pageSize">Amunt of items to get from the given page</param>
        /// <returns>List of found values</returns>
        /// <response code="200">Returns List of found values</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [Route("GetAll")]
        public ActionResult<List<Value>> GetAll([FromQuery]int page = 0, [FromQuery]int pageSize=1000)
        {
            return AllValues.Skip(page* pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Get a Value with the given ID
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/values/Get/1
        ///
        /// </remarks>
        /// <param name="id">Id of the value to fetch</param>
        /// <returns>The value</returns>
        /// <response code="200">The value</response>
        /// <response code="404">Not Found</response>
        [HttpGet]
        [Route("Get/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Value> Get(int id)
        {
            var found= AllValues.FirstOrDefault(x=>x.Id == id);
            if (found!= null)
            {
                return Ok(found);
            }

            return NotFound();
        }

        /// <summary>
        /// Adds a value to the values collection 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/values/Add
        ///     {
        ///        "name":"NewName"
        ///     } 
        ///    
        /// </remarks>
        /// <param name="newValue">the value to insert</param>
        /// <returns>Return the inserted value</returns>
        /// <response code="200">Return the inserted value</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [Route("Add")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<Value> Add([FromBody] Value newValue)
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
                
                return Ok(newValue);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Adds a value to the values collection 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/values/Add
        ///     {
        ///        "id":1,
        ///        "name":"UpdateName"
        ///     } 
        ///    
        /// </remarks>
        /// <param name="updateValue">The value to update</param>
        /// <returns>Return the updateValue value</returns>
        /// <response code="200">Return the updateValue value</response>
        /// <response code="404">Not Found</response>
        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Value> Update( [FromBody] Value updateValue)
        {
            var found = AllValues.FirstOrDefault(x => x.Id == updateValue.Id);
            if (found != null)
            {
                found.Name = updateValue.Name;
                return Ok(updateValue);
            }
            return NotFound();
        }

        // DELETE api/values/Delete/5
        [HttpDelete]
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
