using Dapper;
using BlogApi.Context;
using BlogApi.Models;
using BlogApi.Dtos;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace BlogApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DapperContext _context;

        public CategoryRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateCategory(Category category)
        {
            var sql = "INSERT INTO [dbo].[Category] ([Name], [Description]) VALUES (@Name, @Description); " +
                      "SELECT SCOPE_IDENTITY();";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql, category);
            }
        }

        public async Task<int> CheckIfCategoryExists(string name)
        {
            var sql = "SELECT * FROM [dbo].[Category] WHERE [Name] = @Name;";

            using (var con = _context.CreateConnection())
            {
                return await con.QueryFirstOrDefaultAsync<int>(sql, new { Name = name });
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var sql = "SELECT * FROM [dbo].[Category]";

            using (var con = _context.CreateConnection())
            {
                return await con.QueryAsync<Category>(sql);
            }
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var sql = "SELECT * FROM [dbo].[Category] WHERE [Id] = @id";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<Category>(sql, new { id });
            }
        }

        public async Task<int> UpdateCategory(Category category)
        {
            var sql = "UPDATE [dbo].[Category] SET [Name] = @Name, [Description] = @Description WHERE [Id] = @Id;";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql, new { category.Name, category.Description, category.Id });
            }
        }

        public async Task<int> DeleteCategory(int id)
        {
            var sql = "DELETE FROM [dbo].[Category] WHERE [Id] = @id";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteScalarAsync<int>(sql, new { id });
            }
        }

        public async Task<IEnumerable<PostUserDto>> GetAllPostsOfCategory(int id)
        {
            var sql = "SELECT p.Id, p.Title, p.Content, p.Status, p.DateTimeCreated, p.DateTimeUpdated, c.Id, c.Name, c.Description, u.Id, u.Username, u.FirstName, u.LastName, u.EmailAddress, u.DateTimeCreated FROM [dbo].[Post] as p " +
                      "LEFT JOIN [dbo].[PostCategory] as pc ON p.Id = pc.postId " +
                      "LEFT JOIN [dbo].[Category] as c ON pc.categoryId = c.Id " +
                      "LEFT JOIN [dbo].[User] as u ON p.userId = u.Id " +
                      "WHERE pc.postId IN (SELECT DISTINCT PostId FROM [dbo].[PostCategory] WHERE categoryId = @Id)";

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
                    groupedPost.Categories = post.SelectMany(post => post.Categories)
                                                 .Where(post => post != null)
                                                 .GroupBy(category => category.Id)
                                                 .Select(category => category.First())
                                                 .ToList();

                    return groupedPost;
                });

                return result;
            }
        }
    }
}
