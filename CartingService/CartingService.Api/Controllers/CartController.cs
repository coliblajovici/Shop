using CartingService.Application.Interfaces;
using CartingService.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/cart")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICartService _cartService;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Returns metadata in the header of the response that describes what other methods
        /// and operations are supported at this URL
        /// </summary>
        /// <returns>Supported methods in header of response</returns>
        [HttpOptions]
        public IActionResult GetOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,DELETE");
            return Ok();
        }

        /// <summary>
        /// Returns the cart info by the unique cart key
        /// </summary>        
        /// <response code="200">The cart was found</response>
        /// <response code="404">The cart was not found</response>
        /// <response code="500">A server fault occurred</response>
        [HttpGet("{cartId:Guid}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCartInfo([FromRoute] Guid cartId)
        {
            var cart = _cartService.GetCart(cartId);

            if (cart == null)
            {
                _logger.LogWarning($"Cart with Id {cartId} cannot be found.");
                return NotFound();
            }

            return Ok(cart);
        }

        /// <summary>
        /// Returns the cart info by the unique cart key
        /// </summary>           
        /// <response code="200">The cart was found</response>
        /// <response code="404">The cart was not found</response>
        /// <response code="500">A server fault occurred</response>
        [HttpGet("{cartId:Guid}")]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCartInfoV2([FromRoute] Guid cartId)
        {
            var cart = _cartService.GetCart(cartId);

            if (cart == null)
            {
                _logger.LogWarning($"Cart with Id {cartId} cannot be found.");
                return NotFound();
            }

            return Ok(cart.CartItems);
        }

        /// <summary>
        /// Adds a new item to the cart
        /// </summary>
        /// <response code="200">The item was created successfully</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="500">A server fault occurred</response>
        [HttpPost("{cartId:Guid}/items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddItem([FromRoute] Guid cartId, [FromBody] CartItem cartItem)
        {
            var item = new CartingService.Domain.Entities.CartItem();
            item.Id = cartItem.Id;
            item.Name = cartItem.Name;
            item.Quantity = cartItem.Quantity;
            item.Price = cartItem.Price;

            _cartService.AddItem(cartId, item);
            _logger.LogInformation($"Added item {item.Id}, {item.Name} to cart with Id {cartId}.");

            return Ok();
        }

        /// <summary>
        /// Deletes the cart item
        /// </summary>
        /// <response code="200">The item was deleted successfully.</response>
        /// <response code="404">The cart or cart item was not found</response>
        /// <response code="500">A server fault occurred</response>
        [HttpDelete("{cartId:Guid}/items/{cartItemId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult RemoveItem([FromRoute] Guid cartId, [FromRoute] int cartItemId)
        {
            var cart = _cartService.GetCart(cartId);

            if (cart == null)
            {
                _logger.LogWarning($"Cart with Id {cartId} cannot be found.");
                return NotFound();
            }

            _cartService.RemoveItem(cartId, cartItemId);
            _logger.LogInformation($"Removed item {cartItemId} from cart with Id {cartId}.");

            return Ok();
        }
    }
}