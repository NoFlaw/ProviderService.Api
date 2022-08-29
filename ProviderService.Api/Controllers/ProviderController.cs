using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProviderService.Api.Models;
using ProviderService.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ProviderService.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService _providerService;
        private readonly ILogger<ProviderController> _logger;

        public ProviderController(ILogger<ProviderController> logger, IProviderService providerService)
        {
            _logger = logger;
            _providerService = providerService;
        }

        /// <summary>
        /// Gets all existing Providers.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("~/api/Provider/GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Provider>> GetAll()
        {
            try
            {
                _logger.LogInformation("WebApi ProviderController: GetAll() - Invoked");

                var providers = await _providerService.GetProviders();

                if (providers == null || !providers.Any())
                    return NotFound();

                return Ok(providers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WebApi ProviderController: GetAll() - Error Captured");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Gets an existing Provider by its identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("~/api/Provider/Get/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Provider>> Get(int id)
        {
            try
            {
                _logger.LogInformation("WebApi ProviderController: Get(int id) - Invoked");

                if (id <= 0)
                    return BadRequest();

                var provider = await _providerService.GetProviderById(id);

                if (provider == null)                
                    return NotFound();                

                return Ok(provider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WebApi Contact ProviderController: Get(int id) - Error Captured");
               return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Adds a new Provider.
        /// </summary>
        /// <param name="provider"></param>
        /// <remarks>
        /// Maybe you wanted to provide more detail for a sample request:
        /// 
        ///     POST api/Provider/Post
        ///     {        
        ///       "id": "1",
        ///       "companyName": "SomeCompanyName",
        ///       "alternateIdentifier": "3fa85f64-5717-4562-b3fc-2c963f66afa6", --GUID here...
        ///       "isActive": true
        ///     }
        /// </remarks>
        /// <returns>Returns a newly created Provider</returns>
        /// <response code="201">Returns the newly created Provider</response>
        /// <response code="400">If the Provider provided as a parameter is null</response>    
        /// <response code="500">If an exception is thrown while trying to execute the operation</response>    
        [HttpPost, Route("~/api/Provider/Post")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Provider>> Post([FromBody] Provider provider)
        {
            try
            {
                _logger.LogInformation("WebApi ProviderController: Post(Provider provider) - Invoked");

                if (provider == null)
                    return BadRequest();

                var success = await _providerService.AddProvider(provider);

                if (!success)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Unexpexted error occurred");

                return CreatedAtAction(nameof(Get), new { id = provider.Id }, provider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WebApi Contact ProviderController: Post(Provider provider) - Error Captured");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Updates an existing Provider.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        [HttpPut, Route("~/api/Provider/Put")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Provider>> Put([FromBody] Provider provider)
        {
            try
            {
                _logger.LogInformation("WebApi ProviderController: Put(Provider provider) - Invoked");

                if (provider == null)
                    return BadRequest();

                if (provider.Id <= 0)
                    return NotFound();

                var success = await _providerService.UpdateProvider(provider);

                if (!success)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Unexpexted error occurred");

                return Ok(provider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WebApi Contact ProviderController: Post(Provider provider) - Error Captured");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Deletes an existing Provider by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("~/api/Provider/Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                var provider = await _providerService.GetProviderById(id);

                if (provider == null)
                    return NotFound();

                var success = await _providerService.Remove(provider);

                if (!success)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Unexpexted error occurred");

                return Ok(success);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
