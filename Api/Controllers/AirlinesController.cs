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
    public class CountriesController : SiscBaseController
    {
        private ICountriesManager _countriesManager;
        private readonly CancellationTokenSource _cancellationTokenSource;
        readonly CancellationToken _cancellationToken;

        
        public CountriesController(ICountriesManager countriesManager)
        {
            _countriesManager = countriesManager;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }
        /// <summary>
        /// Gets a Paged amount of countries 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/countries/GetAllAsync
        ///     {
        ///        "pageIndex":0,
        ///        "pageSize":100,
        ///        "orderByColumns":[
        ///             {
        ///                 "columnName":"name",
        ///                 "descending":true
        ///             },
        ///             {
        ///                 "columnName":"isoCode",
        ///                 "descending":false
        ///             }
        ///         ]
        ///     } 
        /// </remarks>
        /// <param name="queryParams">BaseQueryParams with paging info and Columns sorting info</param>
        /// <returns>List of found countries</returns>
        /// <response code="200">Returns List of found countries
        ///         [
        ///             {
        ///                 "id": 1,
        ///                 "name": "Colombia",
        ///                 "isoCode": "CO",
        ///             }
        ///         ]
        /// </response>
    [HttpPost]
    [Produces("application/json")]
        [ProducesResponseType(200)]
        [Route("GetAllAsync")]
        public async Task<ActionResult<List<Country>>> GetAllAsync([FromBody] BaseQueryParams queryParams)
        {
            try
            {
                return await _countriesManager.GetAllAsync(queryParams, _cancellationToken);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debugger.Log(1,"Error", exception.Message);
                return null;
            }
        }

        /// <summary>
        /// Get a country with the given ID
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/v1/countries/Get/1
        ///
        /// </remarks>
        /// <param name="id">Id of the country to fetch</param>
        /// <returns>The country</returns>
        /// <response code="200">The country</response>
        /// <response code="404">Not Found</response>
        [HttpGet]
        [Route("GetAsync/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Country>> GetAsync(int id)
        {
          
            var found = await _countriesManager.GetAsync(new object[] { id }, _cancellationToken);
            if (found != null)
            {
                return Ok(found);
            }

            return NotFound();
            
        }
        /// <summary>
        /// Get a country with the given ISO code
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/v1/countries/GetByIataCodeAsync/col
        ///
        /// </remarks>
        /// <param name="isoCode">ISO code of the country to fetch</param>
        /// <returns>The country</returns>
        /// <response code="200">The country</response>
        /// <response code="404">Not Found</response>
        [HttpGet]
        [Route("GetByIsoCodeAsync/{isoCode}")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Country>> GetByIsoCodeAsync(string isoCode)
        {
            var found = await _countriesManager.GetByIsoCodeAsync(isoCode, _cancellationToken);
            if (found != null)
            {
                return Ok(found);
            }

            return NotFound();
        }
        /// <summary>
        /// Adds an Country to the countries collection 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/countries/AddAsync
        ///     {
        ///        "name":"Suiza",
        ///        "isoCode":"CH",
        ///     } 
        ///    
        /// </remarks>
        /// <param name="newCountry">the Country to insert</param>
        /// <returns>Return the inserted Country</returns>
        /// <response code="200">Return the inserted Country</response>
        /// <response code="400">Bad Request</response>
        [HttpPost]
        [Route("AddAsync")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Country>> AddAsync([FromBody] Country newCountry)
        {
            try
            {
                //TODO: Find out outnumber on postgres
                await _countriesManager.SaveNewAsync(newCountry, _cancellationToken);

                return Ok(newCountry);
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debugger.Log(1, "Error", exception.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Updates a country to the countries collection. Updates all fields 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/countries/UpdateAsync
        ///     {
        ///        "id":1,
        ///        "name":"Colombia",
        ///        "isoCode":"CO"
        ///     } 
        ///    
        /// </remarks>
        /// <param name="updateCountry">The Country to update</param>
        /// <returns>Return the updated Country</returns>
        /// <response code="200">Return the updated Country</response>
        /// <response code="404">Not Found</response>
        [HttpPut]
        [Route("PutUpdateAsync")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Country>> PutUpdateAsync([FromBody] Country updateCountry)
        {
            var returnValue = await UpdateCountry(updateCountry, HttpMethods.Put);
            if (returnValue > 0)
            {
                return Ok(updateCountry);
            }
            return NotFound();
        }
        /// <summary>
        /// Updates an Country to the countries collection. Updates only fields not null 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH api/v1/countries/UpdateAsync
        ///     {
        ///        "id":1,
        ///        "name":"Colombia"
        ///     } 
        ///    
        /// </remarks>
        /// <param name="updateCountry">The Country to update</param>
        /// <returns>Return the updated Country</returns>
        /// <response code="200">Return the updated Country</response>
        /// <response code="404">Not Found</response>
        [HttpPatch]
        [Route("PatchUpdateAsync")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Country>> PatchUpdateAsync([FromBody] Country updateCountry)
        {
            var returnValue = await UpdateCountry(updateCountry, HttpMethods.Put);
            if (returnValue > 0)
            {
                return Ok(updateCountry);
            }
            return NotFound();
        }

        private async Task<int> UpdateCountry(Country updateCountry, string method)
        {
            Country country = null;
            if (method == HttpMethods.Put)
            {
                country = updateCountry;
            }
            else if (method == HttpMethods.Patch)
            {
                country = new Country {Id = updateCountry.Id};
                if (string.IsNullOrEmpty(updateCountry.IsoCode))
                {
                    country.IsoCode = updateCountry.IsoCode;
                }
                if (string.IsNullOrEmpty(updateCountry.Name))
                {
                    country.Name = updateCountry.Name;
                }
            }
            return await _countriesManager.UpdateAsync(country, _cancellationToken);

        }
        /// <summary>
        /// Delete a country from the countries collection.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete api/v1/countries/DeleteAsync/1
        ///    
        /// </remarks>
        /// <param name="id">The id of the Country to delete</param>
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
            var returnValue = await _countriesManager.RemoveAsync(new Country { Id = id }, _cancellationToken);

            if (returnValue > 0)
            {
                return NoContent();
            }
            return NotFound();
        }

    }
}