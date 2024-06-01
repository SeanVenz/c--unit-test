using AutoMapper;
using BlogApi.Dtos;
using BlogApi.Models;
using BlogApi.Repositories;

namespace BlogApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Category> CreateCategory(CategoryCreationDto category)
        {
            var model = _mapper.Map<Category>(category);
            model.Id = await _repository.CreateCategory(model);

            return model;
        }

        public Task<int> CheckIfCategoryExists(string name)
        {
            return _repository.CheckIfCategoryExists(name);
        }

        public Task<IEnumerable<Category>> GetAllCategories()
        {
            return _repository.GetAllCategories();
        }

        public Task<Category> GetCategoryById(int id)
        {
            return _repository.GetCategoryById(id);
        }

        public async Task<int> UpdateCategory(int id, CategoryUpdationDto category)
        {
            var model = _mapper.Map<Category>(category);
            model.Id = id;

            return await _repository.UpdateCategory(model);
        }

        public async Task<int> DeleteCategory(int id)
        {
            return await _repository.DeleteCategory(id);
        }

        public Task<IEnumerable<PostUserDto>> GetAllPostsOfCategory(int id)
        {
            return _repository.GetAllPostsOfCategory(id);
        }
    }
}
