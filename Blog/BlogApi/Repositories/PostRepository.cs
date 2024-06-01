using Dapper;
using BlogApi.Context;
using BlogApi.Models;
using System.Data;
using System.Xml.Linq;
using BlogApi.Dtos;
using Microsoft.Extensions.Hosting;

namespace BlogApi.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DapperContext _context;

        public PostRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreatePost(Post post)
        {
            var spName = "[spPost_CreatePost]";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(
                    spName,
                    new {
                        UserId = post.UserId,
                        Title = post.Title,
                        Content = post.Content,
                        Categories = post.Categories,
                        Status = post.Status,
                        DateTimeCreated = post.DateTimeCreated,
                        DateTimeUpdated = post.DateTimeUpdated
                    },
                    commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<IEnumerable<PostUserDto>> GetAllPostsByStatus(PostGetDto post)
        {
            var sql = "SELECT p.Id, p.Title, p.Content, p.Status, p.DateTimeCreated, p.DateTimeUpdated, c.Id, c.Name, c.Description, u.Id, u.Username, u.FirstName, u.LastName, u.EmailAddress, u.DateTimeCreated FROM [dbo].[Post] as p " +
                      "LEFT JOIN [dbo].[PostCategory] as pc ON p.Id = pc.postId " +
                      "LEFT JOIN [dbo].[Category] as c ON pc.categoryId = c.Id " +
                      "LEFT JOIN [dbo].[User] as u ON p.userId = u.Id " +
                      "WHERE p.Status = @Status;";

            using (var connection = _context.CreateConnection())
            {
                var posts = await connection.QueryAsync<PostUserDto, Category, UserDto, PostUserDto>(sql, (post, category, user) =>
                {
                    post.Categories.Add(category);
                    post.User = user;

                    return post;
                }, new { Status = post.Status });

                var result = posts.GroupBy(post => post.Id).Select(post =>
                {
                    var groupedPost = post.First();
                    groupedPost.Categories = post.SelectMany(post => post.Categories).Where(category => category != null).ToList();

                    return groupedPost;
                });

                return result;
            }
        }

        public async Task<IEnumerable<PostUserDto>> GetAllPosts()
        {
            var sql = "SELECT p.Id, p.Title, p.Content, p.Status, p.DateTimeCreated, p.DateTimeUpdated, c.Id, c.Name, c.Description, u.Id, u.Username, u.FirstName, u.LastName, u.EmailAddress, u.DateTimeCreated FROM [dbo].[Post] as p " +
                      "LEFT JOIN [dbo].[PostCategory] as pc ON p.Id = pc.postId " +
                      "LEFT JOIN [dbo].[Category] as c ON pc.categoryId = c.Id " +
                      "LEFT JOIN [dbo].[User] as u ON p.userId = u.Id;";

            using (var connection = _context.CreateConnection())
            {
                var posts = await connection.QueryAsync<PostUserDto, Category, UserDto, PostUserDto>(sql, (post, category, user) =>
                {
                    post.Categories.Add(category);
                    post.User = user;

                    return post;
                });

                var result = posts.GroupBy(post => post.Id).Select(post =>
                {
                    var groupedPost = post.First();
                    groupedPost.Categories = post.SelectMany(post => post.Categories).Where(post => post != null).ToList();

                    return groupedPost;
                });

                return result;
            }
        }

        public async Task<PostUserDto?> GetPostById(int id)
        {
            var sql = "SELECT p.Id, p.Title, p.Content, p.Status, p.DateTimeCreated, p.DateTimeUpdated, c.Id, c.Name, c.Description, u.Id, u.Username, u.FirstName, u.LastName, u.EmailAddress, u.DateTimeCreated FROM [dbo].[Post] as p " +
                      "LEFT JOIN [dbo].[PostCategory] as pc ON p.Id = pc.postId " +
                      "LEFT JOIN [dbo].[Category] as c ON pc.categoryId = c.Id " +
                      "LEFT JOIN [dbo].[User] as u ON p.userId = u.Id " +
                      "WHERE p.Id = @Id;";

            using (var connection = _context.CreateConnection())
            {
                var posts = await connection.QueryAsync<PostUserDto, Category, UserDto, PostUserDto>(sql, (post, category, user) =>
                {
                    post.Categories.Add(category);
                    post.User = user;

                    return post;
                }, new { Id = id });

                var result = posts.GroupBy(post => post.Id).Select(post =>
                {
                    var groupedPost = post.First();
                    groupedPost.Categories = post.SelectMany(post => post.Categories).Where(post => post != null).ToList();

                    return groupedPost;
                });

                return result.FirstOrDefault();
            }
        }

        public async Task<int> UpdatePost(Post post)
        {
            var spName = "[spPost_UpdatePost]";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(
                    spName,
                    new
                    {
                        PostId = post.Id,
                        Title = post.Title,
                        Content = post.Content,
                        Categories = post.Categories,
                        Status = post.Status,
                        DateTimeUpdated = post.DateTimeUpdated
                    },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> DeletePost(int id)
        {
            var sql = "DELETE FROM [dbo].[Post] WHERE [Id] = @Id";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql, new { Id = id });
            }
        }
    }
}

