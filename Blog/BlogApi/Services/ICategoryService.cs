using BlogApi.Dtos;
using BlogApi.Models;

namespace BlogApi.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// Creates a new category
        /// </summary>
        /// <param name="category">Category details</param>
        /// <returns>Returns the category model</returns>
        Task<Category> CreateCategory(CategoryCreationDto category);

        /// <summary>
        /// Checks if the category already exists
        /// </summary>
        /// <param name="name">The name of the category</param>
        /// <returns>
        /// Returns an integer (0 means that the category already exists, while greater
        /// than 0 means that the category still doesn't exist)
        /// </returns>
        Task<int> CheckIfCategoryExists(string name);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>Returns all categories</returns>
        Task<IEnumerable<Category>> GetAllCategories();

        /// <summary>
        /// Gets the category by id
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>Returns the details of category with id <paramref name="id"/></returns>
        Task<Category> GetCategoryById(int id);

        /// <summary>
        /// Updates an existing category
        /// </summary>
        /// <param name="id">The id of the category that will be updated</param>
        /// <param name="category">New category details</param>
        /// <returns>
        /// Returns an integer (0 means that the update failed, while greater
        /// than 0 means that the category was updated successfully)
        /// </returns>
        Task<int> UpdateCategory(int id, CategoryUpdationDto category);

        /// <summary>
        /// Deletes an existing category
        /// </summary>
        /// <param name="id">The id of the category that will be deleted</param>
        /// <returns>
        /// Returns an integer (0 means that the delete failed, while greater
        /// than 0 means that the category was deleted successfully)
        /// </returns>
        Task<int> DeleteCategory(int id);

        /// <summary>
        /// Gets all posts of category
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns>Returns all posts of category with id <paramref name="id"/></returns>
        Task<IEnumerable<PostUserDto>> GetAllPostsOfCategory(int id);
    }
}
