using BlogApi.Context;
using BlogApi.Dtos;
using BlogApi.Models;
using Dapper;

namespace BlogApi.Repositories
{
    public class CommentRepository: ICommentRepository
    {
        private readonly DapperContext _context;

        public CommentRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateComment(Comment comment)
        {
            var sql = "INSERT INTO [dbo].[Comment] ([UserId], [PostId], [Content], [DateTimeCreated], [DateTimeUpdated]) " +
                      "VALUES (@UserId, @PostId, @Content, @DateTimeCreated, @DateTimeUpdated); " +
                      "SELECT SCOPE_IDENTITY()";

            using (var con = _context.CreateConnection()) 
            {
                return await con.ExecuteScalarAsync<int>(sql, comment);
            }
        }

        public async Task<IEnumerable<Comment>> GetAllComments(int id)
        {
            var sql = "SELECT * FROM [dbo].[Comment] WHERE [PostId] = @Id";

            using (var con = _context.CreateConnection())
            {
                return await con.QueryAsync<Comment>(sql, new { id });
            }
        }

        public async Task<Comment> GetCommentById(int id)
        {
            var sql = "SELECT * FROM [dbo].[Comment] WHERE [Id] = @Id;";

            using (var con = _context.CreateConnection())
            {
                return await con.QuerySingleOrDefaultAsync<Comment>(sql, new { Id = id });
            }
        }

        public async Task<int> UpdateComment(Comment comment)
        {
            var sql = "UPDATE [dbo].[Comment] SET [Content] = @Content, [DateTimeUpdated] = @DateTimeUpdated WHERE [Id] = @Id";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteAsync(
                    sql,
                    new
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        DateTimeUpdated = comment.DateTimeUpdated,
                    }
                );
            }
        }

        public async Task<int> DeleteComment(int postId, int id)
        {
            var sql = "DELETE FROM [dbo].[Comment] WHERE [PostId] = @PostId AND [Id] = @Id";

            using (var con = _context.CreateConnection())
            {
                return await con.ExecuteAsync(sql, new { PostId = postId, Id = id });
            }
        }
    }
}
