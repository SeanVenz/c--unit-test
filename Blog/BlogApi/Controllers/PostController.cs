using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BlogApi.Dtos;
using BlogApi.Services;

namespace BlogApi.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostService postService, ICommentService commentService, ILogger<PostController> logger)
        {
            _postService = postService;
            _commentService = commentService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new post
        /// </summary>
        /// <param name="post">Post details</param>
        /// <returns>Returns the newly created post</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/posts
        ///     {
        ///         "userid": 1,
        ///         "title": "Coaching Advice",
        ///         "content": "&lt;p&gt;&lt;img src=\"https:\/\/ckeditor.com\/apps\/ckfinder\/userfiles\/files\/image-20221018163933-1.png\" style=\"height:340px; width:340px\"\/&gt;&lt;\/p&gt;&lt;p&gt;&amp;nbsp;&lt;\/p&gt;&lt;p&gt;&lt;strong&gt;PREPARING FOR BASKETBALL SEASON-COACHING ADVICE&lt;\/strong&gt;&lt;\/p&gt;&lt;p&gt;The most challenging seasons are the ones that leave you unprepared. According to research on preparing for big seasons and events, one of the most effective tools are practice tests.&amp;nbsp;&lt;\/p&gt;&lt;ul&gt;&lt;li&gt;What is the energy in the gym and the level of intensity your coaching inspires?&lt;\/li&gt;&lt;li&gt;How quickly does your team follow through on your instructions?&lt;\/li&gt;&lt;\/ul&gt;",
        ///         "categories": [1, 2],
        ///         "status": "Published"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Successfully created a new post</response>
        /// <response code="400">Post details are invalid</response>
        /// <response code="500">Internal server error</response>
        [HttpPost(Name = "CreatePost")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PostCreationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePost([FromBody] PostCreationDto post)
        {
            try
            {
                // Create the new post
                var newPost = await _postService.CreatePost(post);
                return CreatedAtRoute("GetPostById", new { id = newPost.Id }, newPost);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Gets all posts
        /// </summary>
        /// <param name="post">Contains a Status OPTIONAL Property</param>
        /// <returns>Returns all posts</returns>
        /// <response code="200">Posts found</response>
        /// <response code="204">No posts found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet(Name = "GetAllPosts")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPosts([FromQuery] PostGetDto post)
        {
            try
            {
                // Check if Status property was provided
                var postsFilteredByStatus = await _postService.GetAllPostsByStatus(post);

                if (postsFilteredByStatus.IsNullOrEmpty())
                {
                    // Return all of the posts instead without a Status filter
                    var posts = await _postService.GetAllPosts();
                    return Ok(posts);
                }

                return Ok(postsFilteredByStatus);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Gets the post by id
        /// </summary>
        /// <param name="id">Post id</param>
        /// <returns>Returns the details of post with id <paramref name="id"/></returns>
        /// <response code="200">Post found</response>
        /// <response code="404">Post not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}", Name = "GetPostById")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                // Check if post exists
                var foundPost = await _postService.GetPostById(id);

                if (foundPost == null)
                {
                    return StatusCode(404, "Post not found");
                }

                return Ok(foundPost);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Updates an existing post
        /// </summary>
        /// <param name="id">The id of the post that will be updated</param>
        /// <param name="post">New post details</param>
        /// <returns>Returns the details of post with id <paramref name="id"/></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/posts
        ///     {
        ///         "title": "Coaching Advice",
        ///         "content": "&lt;p&gt;&lt;img src=\"https:\/\/ckeditor.com\/apps\/ckfinder\/userfiles\/files\/image-20221018163933-1.png\" style=\"height:340px; width:340px\"\/&gt;&lt;\/p&gt;&lt;p&gt;&amp;nbsp;&lt;\/p&gt;&lt;p&gt;&lt;strong&gt;PREPARING FOR BASKETBALL SEASON-COACHING ADVICE&lt;\/strong&gt;&lt;\/p&gt;&lt;p&gt;The most challenging seasons are the ones that leave you unprepared. According to research on preparing for big seasons and events, one of the most effective tools are practice tests.&amp;nbsp;&lt;\/p&gt;&lt;ul&gt;&lt;li&gt;What is the energy in the gym and the level of intensity your coaching inspires?&lt;\/li&gt;&lt;li&gt;How quickly does your team follow through on your instructions?&lt;\/li&gt;&lt;\/ul&gt;",
        ///         "categories": [1, 2],
        ///         "status": "Published"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Successfully updated the post</response>
        /// <response code="404">Post not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}", Name = "UpdatePost")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PostUpdationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] PostUpdationDto post)
        {
            try
            {
                // Check if post exists
                var foundPost = await _postService.GetPostById(id);

                if (foundPost == null)
                {
                    return StatusCode(404, "Post not found");
                }

                // Update the post
                await _postService.UpdatePost(id, post);
                return Ok("Post updated successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Deletes an existing post
        /// </summary>
        /// <param name="id">The id of the post that will be deleted</param>
        /// <response code="200">Successfully deleted the post</response>
        /// <response code="404">Post not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}", Name = "DeletePost")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                // Check if post exists
                var foundPost = await _postService.GetPostById(id);

                if (foundPost == null)
                {
                    return StatusCode(404, "Post not found");
                }

                await _postService.DeletePost(id);
                return Ok("Post deleted successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Creates a new comment
        /// </summary>
        /// <param name="id">Post id</param>
        /// <param name="comment">Comment details</param>
        /// <returns>Returns the newly created comment</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/posts/1/comments
        ///     {
        ///         "userid": 1,
        ///         "content": "Niceeee Good Game"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Successfully created a new comment</response>
        /// <response code="400">Comment details are invalid</response>
        /// <response code="404">Post not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("{id}/comments")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommentCreationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateComment(int id, [FromBody] CommentCreationDto comment)
        {
            try
            {
                // Check if post exists
                var foundPost = await _postService.GetPostById(id);

                if (foundPost == null)
                {
                    return StatusCode(404, "Post not found");
                }

                // Create the new comment
                var newComment = await _commentService.CreateComment(id, comment);
                return Ok(newComment);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Gets all Comments
        /// </summary>
        /// <param name="id">Post id</param>
        /// <returns>Returns all comments</returns>
        /// <response code="200">Comments found</response>
        /// <response code="204">No comments found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}/comments")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllComments(int id)
        {
            try
            {
                // Check if post exists
                var foundPost = await _postService.GetPostById(id);

                if (foundPost == null)
                {
                    return StatusCode(404, "Post not found");
                }

                // Get all comments
                var comments = await _commentService.GetAllComments(id);

                if (comments.IsNullOrEmpty())
                {
                    return NoContent();
                }

                return Ok(comments);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Updates an existing comment
        /// </summary>
        /// <param name="id">The id of the post that will be updated</param>
        /// <param name="commentId">The id of the comment that will be updated</param>
        /// <param name="comment">New comment details</param>
        /// <returns>Returns the details of comment with id <paramref name="id"/></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/posts/1/comments/1
        ///     {
        ///         "content": "This is amazing!"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Successfully updated the comment</response>
        /// <response code="404">Post/Comment not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}/comments/{commentId}", Name = "UpdateComment")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommentUpdationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateComment(int id, int commentId, [FromBody] CommentUpdationDto comment)
        {
            try
            {
                // Check if post exists
                var foundPost= await _postService.GetPostById(id);

                if (foundPost == null)
                {
                    return StatusCode(404, "Post not found");
                }

                // Check if comment exists
                var foundComment = await _commentService.GetCommentById(commentId);

                if (foundComment == null)
                {
                    return StatusCode(404, "Comment not found");
                }

                // Update the comment
                await _commentService.UpdateComment(id, commentId, comment);
                return Ok("Comment updated successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Deletes an existing comment
        /// </summary>
        /// <param name="id">Post id</param>
        /// <param name="commentId">The id of the comment that will be deleted</param>
        /// <response code="200">Successfully updated the comment</response>
        /// <response code="404">Post/Comment not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}/comments/{commentId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteComment(int id, int commentId)
        {
            try
            {
                // Check if post exists
                var foundPost = await _postService.GetPostById(id);

                if (foundPost == null)
                {
                    return StatusCode(404, "Post not found");
                }

                // Check if comment exists
                var foundComment = await _commentService.GetCommentById(commentId);

                if (foundComment == null)
                {
                    return StatusCode(404, "Comment not found");
                }

                await _commentService.DeleteComment(id, commentId);
                return Ok("Comment deleted successfully");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
