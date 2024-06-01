using AutoMapper;
using BlogApi.Dtos;
using BlogApi.Models;
using BlogApi.Repositories;
using BlogApi.Utils;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace BlogApi.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _repository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Post> CreatePost(PostCreationDto post)
        {
            var model = _mapper.Map<Post>(post);
            model.Id = await _repository.CreatePost(model);

            return model;
        }

        public Task<IEnumerable<PostUserDto>> GetAllPostsByStatus(PostGetDto post)
        {
            return _repository.GetAllPostsByStatus(post);
        }

        public Task<IEnumerable<PostUserDto>> GetAllPosts()
        {
            return _repository.GetAllPosts();
        }

        public Task<PostUserDto?> GetPostById(int id)
        {
            return _repository.GetPostById(id);
        }

        public async Task<int> UpdatePost(int id, PostUpdationDto post)
        {
            var model = _mapper.Map<Post>(post);
            model.Id = id;

            return await _repository.UpdatePost(model);
        }

        public async Task<int> DeletePost(int id)
        {
            return await _repository.DeletePost(id);
        }
    }
}

