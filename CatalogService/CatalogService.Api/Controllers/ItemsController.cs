using CatalogService.Api.Dto;
using CatalogService.Api.Models;
using CatalogService.Api.Models.Interfaces;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using ShopServiceBusClient;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Http.Filters;

namespace CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/items")]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public class ItemsController : ControllerBase
    {       
        private readonly ILogger<ItemsController> _logger;
        private readonly IProductService _productService;
        private readonly IItemResourceBuilder _itemResourceBuilder;
        private readonly IHttpContextAccessor _contextAccessor;

        private string _currentPrincipalId = string.Empty;


        public ItemsController(IProductService productService, ILogger<ItemsController> logger, IItemResourceBuilder itemResourceBuilder, IHttpContextAccessor contextAccessor)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _itemResourceBuilder = itemResourceBuilder ?? throw new ArgumentNullException(nameof(itemResourceBuilder));
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Returns metadata in the header of the response that describes what other methods
        /// and operations are supported at this URL
        /// </summary>
        /// <returns>Supported methods in header of response</returns>
        [HttpOptions]
        public IActionResult GetOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,PUT,DELETE");
            return Ok();
        }

        /// <summary>
        /// Returns the item by the itemId
        /// </summary>        
        /// <response code="200">The item was found</response>
        /// <response code="404">The item was not found</response>
        [HttpGet("{itemId:int}", Name =nameof(GetItemById))] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ItemResource GetItemById([FromRoute]int itemId)
        {
            var product = _productService.GetProduct(itemId);

            return _itemResourceBuilder.CreateItemResource(product);
        }

        /// <summary>
        /// Returns the list of items
        /// </summary>
        /// <response code="200">Returns the list of items</response>
        [HttpGet(Name =nameof(GetItems))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public IEnumerable<Product> GetItems()
        {
            return _productService.GetProducts();
        }

        /// <summary>
        /// Adds a new item
        /// </summary>
        /// <response code="201">The item was created successfully. Also includes 'location' header to newly created item</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        [HttpPost(Name = nameof(AddItem))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequiredScopeOrAppPermission(
            AcceptedScope = new string[] { "Catalog.Write" }
        )]
        public IActionResult AddItem([FromBody] ProductDto productDto)
        {
            var product = new Product(productDto.Name, productDto.Description, productDto.ImageUrl, productDto.CategoryId, productDto.Price, productDto.Amount);

            var createdProduct = _productService.Add(product);

            return CreatedAtAction(
              actionName: nameof(GetItemById),
              routeValues: new { itemId = createdProduct.Id },
              value: _itemResourceBuilder.CreateItemResource(createdProduct));
        }

        /// <summary>
        /// Deletes am item
        /// </summary>
        /// <response code="204">The item was deleted successfully.</response>
        /// <response code="404">The item was not found</response>
        [HttpDelete("{itemId:int}", Name = nameof(DeleteItem))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequiredScopeOrAppPermission(
            AcceptedScope = new string[] { "Catalog.Write" }
        )]
        public IActionResult DeleteItem([FromRoute] int itemId)
        {
            _productService.Delete(itemId);
            return NoContent();
        }

        /// <summary>
        /// Updates an item
        /// </summary>
        /// <response code="204">The item was updated successfully.</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="404">The item was not found</response>
        [HttpPut("{itemId:int}", Name = nameof(UpdateItem))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequiredScopeOrAppPermission(
            AcceptedScope = new string[] { "Catalog.Write" }
        )]
        public IActionResult UpdateItem([FromRoute] int itemId, [FromBody] ProductDto productDto)
        {
            var product = _productService.GetProduct(itemId);
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.ImageUrl = productDto.ImageUrl;
            product.CategoryId = productDto.CategoryId;
            product.Price = productDto.Price;
            product.SetAmount(productDto.Amount);
            
            _productService.Update(product);
            
            return NoContent();
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