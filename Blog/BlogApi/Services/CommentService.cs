using AutoMapper;
using BlogApi.Dtos;
using BlogApi.Models;
using BlogApi.Repositories;

namespace BlogApi.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Comment> CreateComment(int postId, CommentCreationDto comment)
        {
            var model = _mapper.Map<Comment>(comment);
            model.PostId = postId;
            model.Id = await _repository.CreateComment(model);

            return model;
        }

        public async Task<IEnumerable<Comment>> GetAllComments(int id)
        {
            return await _repository.GetAllComments(id);
        }

        public Task<Comment> GetCommentById(int id)
        {
            return _repository.GetCommentById(id);
        }

        public async Task<int> UpdateComment(int postId, int id, CommentUpdationDto comment)
        {
            var model = _mapper.Map<Comment>(comment);
            model.PostId = postId;
            model.Id = id;

            return await _repository.UpdateComment(model);
        }

        public async Task<int> DeleteComment(int postId, int id)
        {
            return await _repository.DeleteComment(postId, id);
        }
    }
}
