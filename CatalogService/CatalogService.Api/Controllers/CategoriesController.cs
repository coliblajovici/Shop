using CatalogService.Api.Dto;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public class CategoriesController : ControllerBase
    {       
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
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
            Response.Headers.Add("Allow", "GET,OPTIONS,POST,PUT,DELETE");
            return Ok();
        }

        /// <summary>
        /// Returns a category by the categoryId
        /// </summary>        
        /// <response code="200">The category was found</response>
        /// <response code="404">The category was not found</response>
        [HttpGet("{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoryById([FromRoute]int categoryId)
        {
            var category = _categoryService.GetCategory(categoryId);

            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        /// <summary>
        /// Returns the list of categories
        /// </summary>
        /// <response code="200">Returns the list of categories</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategoryList()
        {
            return Ok(_categoryService.GetCategories());
        }

        /// <summary>
        /// Adds a new category
        /// </summary>
        /// <response code="201">The category was created successfully. Also includes 'location' header to newly created category</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddCategory([FromBody] CategoryDto categoryDto)
        {
            var category = new Category(categoryDto.Name, categoryDto.ImageUrl);

            var createdCategory = _categoryService.Add(category);

            return CreatedAtAction(
              actionName: nameof(GetCategoryById),
              routeValues: new { categoryId = createdCategory.Id },
              value: createdCategory);
        }

        /// <summary>
        /// Deletes a category
        /// </summary>
        /// <response code="204">The category was deleted successfully.</response>
        /// <response code="404">The category was not found</response>
        [HttpDelete("{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteCategory([FromRoute] int categoryId)
        {
            _categoryService.Delete(categoryId);
            return NoContent();
        }

        /// <summary>
        /// Updates a category
        /// </summary>
        /// <response code="204">The category was updated successfully.</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="404">The category was not found</response>
        [HttpPut("{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateCategory([FromRoute] int categoryId, [FromBody] CategoryDto categoryDto)
        {
            var category = _categoryService.GetCategory(categoryId);
            category.UpdateName(categoryDto.Name);
            category.UpdateImageUrl(categoryDto.ImageUrl);
            
            _categoryService.Update(category);

            return NoContent();
        }
    }
}