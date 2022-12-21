using System.Security.Claims;
using CatalogService.Api.Dto;
using CatalogService.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/item-properties")]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public class ItemPropertiesController : ControllerBase
    {

        public ItemPropertiesController()
        {
        }

        /// <summary>
        /// Returns metadata in the header of the response that describes what other methods
        /// and operations are supported at this URL
        /// </summary>
        /// <returns>Supported methods in header of response</returns>
        [HttpOptions]
        public IActionResult GetOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS");
            return Ok();
        }

        /// <summary>
        /// Returns the item by the itemId
        /// </summary>        
        /// <response code="200">The item was found</response>
        /// <response code="404">The item was not found</response>
        [HttpGet("{itemId:int}", Name = nameof(GetItemPropertiesByItemId))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ProductPropertiesDto GetItemPropertiesByItemId([FromRoute] int itemId)
        {
            var propertyPairs = new Dictionary<string, string>();
            propertyPairs["Name"] = "Lipstick";
            propertyPairs["Description"] = "Very nice lipstick";
            propertyPairs["Brand"] = "Very famous brand";
            propertyPairs["Model"] = "s100";

            return new ProductPropertiesDto() { PropertyPairs = propertyPairs };
        }
    }
}