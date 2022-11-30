using CatalogService.Api.Dto;
using CatalogService.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/item-properties")]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public class ItemPropertiesController : ControllerBase
    {       
        private readonly ILogger<ItemsController> _logger;
        private readonly IProductService _productService;        
        private readonly IHttpContextAccessor _contextAccessor;

        private string _currentPrincipalId = string.Empty;


        public ItemPropertiesController(IProductService productService, ILogger<ItemsController> logger, IHttpContextAccessor contextAccessor)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));            
            _contextAccessor = contextAccessor;

            _currentPrincipalId = GetCurrentClaimsPrincipal()?.GetObjectId();
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
        [HttpGet("{itemId:int}", Name =nameof(GetItemPropertiesByItemId))] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ProductPropertiesDto GetItemPropertiesByItemId([FromRoute]int itemId)
        {
            //var product = _productService.GetProduct(itemId);

            var propertyPairs = new Dictionary<string, string>();
            propertyPairs["Name"] = "Lipstick";
            propertyPairs["Description"] = "Very nice lipstick";            
            propertyPairs["Brand"] = "Very famous brand";
            propertyPairs["Model"] = "s100";

            return new ProductPropertiesDto(){ PropertyPairs = propertyPairs };
        }       

        /// <summary>
        /// returns the current claimsPrincipal (user/Client app) dehydrated from the Access token
        /// </summary>
        /// <returns></returns>
        private ClaimsPrincipal GetCurrentClaimsPrincipal()
        {
            // Irrespective of whether a user signs in or not, the AspNet security middle-ware dehydrates the claims in the
            // HttpContext.User.Claims collection

            if (_contextAccessor.HttpContext != null && _contextAccessor.HttpContext.User != null)
            {
                return _contextAccessor.HttpContext.User;
            }

            return null;
        }
    }
}