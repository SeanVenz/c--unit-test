using Dapper;
using BlogApi.Context;
using BlogApi.Models;
using BlogApi.Dtos;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace BlogApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateUser(User user)
        {
            var sql = "INSERT INTO [dbo].[User] ([Username], [Password], [FirstName], [LastName], [EmailAddress], [DateTimeCreated]) " +
                      "VALUES (@Username, @Password, @FirstName, @LastName, @EmailAddress, @DateTimeCreated); " +
                      "SELECT SCOPE_IDENTITY();";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql, user);
            }
        }

        public async Task<int> CheckIfUserExists(string username)
        {
            var sql = "SELECT * FROM [dbo].[User] WHERE [Username] = @Username;";

            using (var con = _context.CreateConnection())
            {
                return await con.QueryFirstOrDefaultAsync<int>(sql, new { Username = username });
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var sql = "SELECT * FROM [dbo].[User];";

            using (var con = _context.CreateConnection())
            {
                return await con.QueryAsync<UserDto>(sql);
            }
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var sql = "SELECT * FROM [dbo].[User] WHERE [Id] = @Id;";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<UserDto>(sql, new { Id = id });
            }
        }

        public async Task<int> UpdateUser(User user)
        {
            var sql = "UPDATE [dbo].[User] SET " +
                      "[Username] = @Username, " +
                      "[FirstName] = @FirstName, " +
                      "[LastName] = @LastName, " +
                      "[EmailAddress] = @EmailAddress " +
                      "WHERE Id = @Id;";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(
                    sql,
                    new
                    {
                        Id = user.Id,
                        Username = user.Username,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        EmailAddress = user.EmailAddress,
                    }
                );
            }
        }

        public async Task<int> DeleteUser(int id)
        {
            var spName = "[spUser_DeleteUser]";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(
                    spName,
                    new { UserId = id },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<UserPostDto?> GetUserPostsById(int id)
        {
            var sql = "SELECT u.*, p.Id, p.Title, p.Content, p.Status, p.DateTimeCreated, p.DateTimeUpdated, c.Id, c.Name, c.Description FROM [dbo].[User] as u " +
                      "LEFT JOIN [dbo].[Post] as p ON u.Id = p.userId " +
                      "LEFT JOIN [dbo].[PostCategory] as pc ON p.Id = pc.postId " +
                      "LEFT JOIN [dbo].[Category] as c ON pc.categoryId = c.Id " +
                      "WHERE u.Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<UserPostDto, PostDto, Category, UserPostDto>(sql, (user, post, category) =>
                {
                    if (category != null)
                    {
                        post.Categories.Add(category);
                    }
                    
                    user.Posts.Add(post);

                    return user;
                }, new { id });

                var result = users.GroupBy(user => user.Id).Select(user =>
                {
                    var groupedUser = user.First();
                    groupedUser.Posts = user.SelectMany(user => user.Posts)
                                            .Where(post => post != null)
                                            .GroupBy(post => post.Id)
                                            .Select(post => {
                                                var groupedPost = post.First();
                                                groupedPost.Categories = post.SelectMany(post => post.Categories)
                                                                             .Where(category => category != null)
                                                                             .GroupBy(category => category.Id)
                                                                             .Select(category => category.First()).ToList();

                                                return groupedPost;
                                            })
                                            .ToList();

                    return groupedUser;
                });

                return result.SingleOrDefault();
            }
        }
    }
}

