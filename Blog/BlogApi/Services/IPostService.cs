using BlogApi.Dtos;
using BlogApi.Models;

namespace BlogApi.Services
{
    public interface IPostService
    {
        /// <summary>
        /// Creates a new post
        /// </summary>
        /// <param name="post">Post details</param>
        /// <returns>Returns the post model</returns>
        Task<Post> CreatePost(PostCreationDto post);

        /// <summary>
        /// Gets all posts filtered by status
        /// </summary>
        /// <returns>Returns all posts filtered by status</returns>
        Task<IEnumerable<PostUserDto>> GetAllPostsByStatus(PostGetDto post);

        /// <summary>
        /// Gets all posts
        /// </summary>
        /// <returns>Returns all posts</returns>
        Task<IEnumerable<PostUserDto>> GetAllPosts();

        /// <summary>
        /// Gets the post by id
        /// </summary>
        /// <param name="id">Post id</param>
        /// <returns>Returns the details of post with id <paramref name="id"/></returns>
        Task<PostUserDto?> GetPostById(int id);

        /// <summary>
        /// Updates an existing post
        /// </summary>
        /// <param name="id">Post id</param>
        /// <param name="post">New post details</param>
        /// <returns>
        /// Returns an integer (0 means that the update failed, while greater
        /// than 0 means that the post was updated successfully)
        /// </returns>
        Task<int> UpdatePost(int id, PostUpdationDto post);

        /// <summary>
        /// Deletes an existing post
        /// </summary>
        /// <param name="id">The id of the post that will be deleted</param>
        /// <returns>
        /// Returns an integer (0 means that the delete failed, while greater
        /// than 0 means that the post was deleted successfully)
        /// </returns>
        Task<int> DeletePost(int id);
    }
}

