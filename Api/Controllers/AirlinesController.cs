using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sisc.Api.Common;
using Sisc.Api.Common.Helpers;
using Sisc.Api.Lib.Managers;

namespace Sisc.Api.Controllers
{
    [ApiVersion("1")]
    public class AirlinesController : SiscBaseController
    {
        private IAirlinesManager _airlinesManager;
        private readonly CancellationTokenSource _cancellationTokenSource;
        readonly CancellationToken _cancellationToken;

        
        public AirlinesController(IAirlinesManager airlinesManager)
        {
            _airlinesManager = airlinesManager;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }
        /// <summary>
        /// Gets a Paged amount of airlines 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/airlines/GetAllAsync
        ///     {
        ///        "pageIndex":0,
        ///        "pageSize":100,
        ///        "orderByColumns":[
        ///             {
        ///                 "columnName":"name",
        ///                 "descending":true
        ///             },
        ///             {
        ///                 "columnName":"iataCode",
        ///                 "descending":false
        ///             }
        ///         ]
        ///     } 
        /// </remarks>
        /// <param name="queryParams">BaseQueryParams with paging info and Columns sorting info</param>
        /// <returns>List of found airlines</returns>
        /// <response code="200">Returns List of found airlines
        ///         [
        ///             {
        ///                 "id": 1,
        ///                 "name": "American Airlines",
        ///                 "iataCode": "AA",
        ///                 "icaoCode": "AAL"
        ///             }
        ///         ]
        /// </response>
    [HttpPost]
    [Produces("application/json")]
        [ProducesResponseType(200)]
        [Route("GetAllAsync")]
        public async Task<ActionResult<List<Airline>>> GetAllAsync([FromBody] BaseQueryParams queryParams)
        {
            try
            {
                return await _airlinesManager.GetAllAsync(queryParams, _cancellationToken);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debugger.Log(1,"Error", exception.Message);
                return null;
            }
        }

        /// <summary>
        /// Get a airline with the given ID
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/v1/airlines/Get/1
        ///
        /// </remarks>
        /// <param name="id">Id of the airline to fetch</param>
        /// <returns>The airline</returns>
        /// <response code="200">The airline</response>
        /// /// <response code="404">Bad Request</response>
        /// <response code="404">Not Found</response>
        [HttpGet]
        [Route("GetAsync/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(4040)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Airline>> GetAsync(int id)
        {
          
            var found = await _airlinesManager.GetAsync(new object[] { id }, _cancellationToken);
            if (found != null)
            {
                return Ok(found);
            }

            return NotFound();
            
        }
        /// <summary>
        /// Get a airline with the given IATA code
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/v1/airlines/GetByIataCodeAsync/col
        ///
        /// </remarks>
        /// <param name="iataCode">IATA code of the airline to fetch</param>
        /// <returns>The airline</returns>
        /// <response code="200">The airline</response>
        /// <response code="404">Not Found</response>
        [HttpGet]
        [Route("GetByIataCode/{iataCode}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Airline>> GetByIataCodeAsync(string iataCode)
        {
            var found = await _airlinesManager.GetByIataCodeAsync(iataCode, _cancellationToken);
            if (found != null)
            {
                return Ok(found);
            }

            return NotFound();
        }
        /// <summary>
        /// Adds an Airline to the Airlines collection 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/Airline/AddAsync
        ///     {
        ///        "name":"American Airlines",
        ///        "iataCode":"AA",
        ///        "icaoCode":"AAL"
        ///     } 
        ///    
        /// </remarks>
        /// <param name="newAirline">the Airline to insert</param>
        /// <returns>Return the inserted Airline</returns>
        /// <response code="200">Return the inserted Airline</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [Route("AddAsync")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Airline>> AddAsync([FromBody] Airline newAirline)
        {
            try
            {
                //TODO: Find out outnumber on postgres
                await _airlinesManager.SaveNewAsync(newAirline, _cancellationToken);

                return Ok(newAirline);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debugger.Log(1, "Error", exception.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Updates an Airline to the Airlines collection. Updates all fields 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/Airline/UpdateAsync
        ///     {
        ///        "id":1,
        ///        "name":"American Airlines",
        ///        "iataCode":"AA",
        ///        "icaoCode":"AAL",
        ///     } 
        ///    
        /// </remarks>
        /// <param name="updateAirline">The Airline to update</param>
        /// <returns>Return the updateAirline</returns>
        /// <response code="200">Return the updateAirline</response>
        /// <response code="404">Not Found</response>
        [HttpPut]
        [Route("PutUpdateAsync")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Airline>> PutUpdateAsync([FromBody] Airline updateAirline)
        {
            var returnValue = await UpdateAirline(updateAirline, HttpMethods.Put);
            if (returnValue > 0)
            {
                return Ok(updateAirline);
            }
            return NotFound();
        }
        /// <summary>
        /// Updates an Airline to the Airlines collection. Updates only fields not null 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH api/Airline/UpdateAsync
        ///     {
        ///        "id":1,
        ///        "name":"American Airlines",
        ///        "iataCode":"AA",
        ///        "icaoCode":"AAL",
        ///     } 
        ///    
        /// </remarks>
        /// <param name="updateAirline">The Airline to update</param>
        /// <returns>Return the updateAirline</returns>
        /// <response code="200">Return the updateAirline</response>
        /// <response code="404">Not Found</response>
        [HttpPatch]
        [Route("PatchUpdateAsync")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Airline>> PatchUpdateAsync([FromBody] Airline updateAirline)
        {
            var returnValue = await UpdateAirline(updateAirline, HttpMethods.Put);
            if (returnValue > 0)
            {
                return Ok(updateAirline);
            }
            return NotFound();
        }

        private async Task<int> UpdateAirline(Airline updateAirline, string method)
        {
            Airline airline = null;
            if (method == HttpMethods.Put)
            {
                airline = updateAirline;
            }
            else if (method == HttpMethods.Patch)
            {
                airline = new Airline {Id = updateAirline.Id};
                if (string.IsNullOrEmpty(updateAirline.IataCode))
                {
                    airline.IataCode = updateAirline.IataCode;
                }
                if (string.IsNullOrEmpty(updateAirline.IcaoCode))
                {
                    airline.IcaoCode = updateAirline.IcaoCode;
                }
                if (string.IsNullOrEmpty(updateAirline.Name))
                {
                    airline.Name = updateAirline.Name;
                }
            }
            return await _airlinesManager.UpdateAsync(airline, _cancellationToken);

        }
        /// <summary>
        /// Delete an Airline from the Airlines collection.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete api/Airline/DeleteAsync/1
        ///    
        /// </remarks>
        /// <param name="id">The id of the Airline to delete</param>
        /// <returns>Status Code</returns>
        /// <response code="204">No Content</response>
        /// <response code="404">Not Found</response>
        [HttpDelete]
        [Route("DeleteAsync")]
        [Produces("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var returnValue = await _airlinesManager.RemoveAsync(new Airline { Id = id }, _cancellationToken);

            if (returnValue > 0)
            {
                return NoContent();
            }
            return NotFound();
        }

    }
}