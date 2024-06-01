using BlogApi.Dtos;
using BlogApi.Models;

namespace BlogApi.Repositories
{
    public interface ICommentRepository
    {
        /// <summary>
        /// Creates a new comment
        /// </summary>
        /// <param name="comment">Comment details</param>
        /// <returns>Returns the id of the newly created comment</returns>
        Task<int> CreateComment(Comment comment);

        /// <summary>
        /// Gets all comments
        /// </summary>
        /// <param name="id">Post id</param>
        /// <returns>Returns all comments</returns>
        Task<IEnumerable<Comment>> GetAllComments(int id);

        /// <summary>
        /// Gets the comment by id
        /// </summary>
        /// <param name="id">Comment id</param>
        /// <returns>Returns the details of comment with id <paramref name="id"/></returns>
        Task<Comment> GetCommentById(int id);

        /// <summary>
        /// Updates an existing comment
        /// </summary>
        /// <param name="comment">New comment details</param>
        /// <returns>
        /// Returns an integer (0 means that the update failed, while greater
        /// than 0 means that the comment was updated successfully)
        /// </returns>
        Task<int> UpdateComment(Comment comment);

        /// <summary>
        /// Deletes an existing comment
        /// </summary>
        /// <param name="postId">Post id</param>
        /// <param name="id">The id of the comment that will be deleted</param>
        /// <returns>
        /// Returns an integer (0 means that the delete failed, while greater
        /// than 0 means that the comment was deleted successfully)
        /// </returns>
        Task<int> DeleteComment(int postId, int id);
    }
}
