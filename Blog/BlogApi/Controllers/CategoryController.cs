using BlogApi.Dtos;
using BlogApi.Models;
using BlogApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new category
        /// </summary>
        /// <param name="category">Category details</param>
        /// <returns>Returns the newly created category</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/categories
        ///     {
        ///         "name": "Automata",
        ///         "description": "Deterministic Finite Automata"
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">Successfully created a new category</response>
        /// <response code="400">Category details are invalid</response>
        /// <response code="409">Category already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost(Name = "CreateCategory")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CategoryCreationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreationDto category)
        {
            try
            {
                // Check if category already exists
                var exists = await _categoryService.CheckIfCategoryExists(category.Name!);

                if (exists > 0)
                {
                    return StatusCode(409, "Category already exists");
                }
                
                // Create the new category
                var newCategory = await _categoryService.CreateCategory(category);
                return CreatedAtRoute("GetCategoryById", new { id = newCategory.Id }, newCategory);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>Returns all categories</returns>
        /// <response code="200">Categories found</response>
        /// <response code="204">No categories found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet(Name = "GetAllCategories")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategories();

                if (categories.IsNullOrEmpty())
                {
                    return NoContent();
                }

                return Ok(categories);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Gets the category by id
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>Returns the details category with id <paramref name="id"/></returns>
        /// <response code="200">Category found</response>
        /// <response code="404">Category not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}", Name = "GetCategoryById")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                // Check if category exists
                var category = await _categoryService.GetCategoryById(id);

                if (category == null)
                {
                    return StatusCode(404, "Category not found");
                }

                return Ok(category);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Updates an existing category
        /// </summary>
        /// <param name="id">The id of the category that will be updated</param>
        /// <param name="category">New category details</param>
        /// <returns>Returns the details of category with id <paramref name="id"/></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/categories
        ///     {
        ///         "name": "Automata",
        ///         "description": "Deterministic Finite Automata"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Successfully updated the category</response>
        /// <response code="404">Category not found</response>
        /// <response code="409">Category already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}", Name = "UpdateCategory")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserUpdationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdationDto category)
        {
            try
            {
                // Check if category exists
                var foundCategory = await _categoryService.GetCategoryById(id);

                if (foundCategory == null)
                {
                    return StatusCode(404, "Category not found");
                }

                // Check if category already exists
                var idOfExistingCategory = await _categoryService.CheckIfCategoryExists(category.Name!);

                if (idOfExistingCategory != id && idOfExistingCategory > 0)
                {
                    return StatusCode(409, "Category already exists");
                }
                
                // Update the category
                await _categoryService.UpdateCategory(id, category);
                return Ok("Category updated successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Deletes an existing category
        /// </summary>
        /// <param name="id">The id of the category that will be deleted</param>
        /// <response code="200">Successfully updated the category</response>
        /// <response code="404">Category not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}", Name = "DeleteCategory")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                // Check if category exists
                var foundCategory = await _categoryService.GetCategoryById(id);

                if (foundCategory == null)
                {
                    return StatusCode(404, "Category not found");
                }

                await _categoryService.DeleteCategory(id);
                return Ok("Category deleted successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Gets all posts of category
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>Returns all posts of category</returns>
        /// <response code="200">Posts found</response>
        /// <response code="204">No posts found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}/posts", Name = "GetAllPostsOfCategory")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPostsOfCategory(int id)
        {
            try
            {
                // Check if category exists
                var foundCategory = await _categoryService.GetCategoryById(id);

                if (foundCategory == null)
                {
                    return StatusCode(404, "Category not found");
                }

                // Get all posts of category
                var posts = await _categoryService.GetAllPostsOfCategory(id);

                if (posts.IsNullOrEmpty())
                {
                    return NoContent();
                }

                return Ok(posts);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}