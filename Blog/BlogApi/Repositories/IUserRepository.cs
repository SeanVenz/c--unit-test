using BlogApi.Dtos;
using BlogApi.Models;

namespace BlogApi.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user">User details</param>
        /// <returns>Returns the id of the newly created user</returns>
        Task<int> CreateUser(User user);

        /// <summary>
        /// Checks if the user already exists
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <returns>
        /// Returns an integer (0 means that the user already exists, while greater
        /// than 0 means that the user still doesn't exist)
        /// </returns>
        Task<int> CheckIfUserExists(string username);

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>Returns all users</returns>
        Task<IEnumerable<UserDto>> GetAllUsers();

        /// <summary>
        /// Gets the user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Returns the details of user with id <paramref name="id"/></returns>
        Task<UserDto> GetUserById(int id);

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="user">New user details</param>
        /// <returns>
        /// Returns an integer (0 means that the update failed, while greater
        /// than 0 means that the user was updated successfully)
        /// </returns>
        Task<int> UpdateUser(User user);

        /// <summary>
        /// Deletes an existing user
        /// </summary>
        /// <param name="id">The id of the user that will be deleted</param>
        /// <returns>
        /// Returns an integer (0 means that the delete failed, while greater
        /// than 0 means that the user was deleted successfully)
        /// </returns>
        Task<int> DeleteUser(int id);

        /// <summary>
        /// Gets all posts of user
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Returns the details of posts with userid <paramref name="id"/></returns>
        Task<UserPostDto?> GetUserPostsById(int id);
    }
}