using AutoMapper;
using BlogApi.Dtos;
using BlogApi.Models;
using BlogApi.Repositories;
using BlogApi.Services;
using Moq;

namespace BlogApiTests.Service
{
    public class ICommentServiceTest
    {
        private readonly ICommentService _fakeCommentService;
        private readonly Mock<ICommentRepository> _fakeCommentRepository;
        private readonly Mock<IMapper> _fakeMapper;

        public ICommentServiceTest()
        {
            _fakeCommentRepository = new Mock<ICommentRepository>();
            _fakeMapper = new Mock<IMapper>();
            _fakeCommentService = new CommentService(_fakeCommentRepository.Object, _fakeMapper.Object);
        }

        [Fact]
        public async void GetAllComments_HasComments_ReturnsListOfCommentDto()
        {
            // Arrange
            _fakeCommentRepository.Setup(service => service.GetAllComments(1))
                .ReturnsAsync(new List<Comment> { new Comment() });

            // Act 
            var result = await _fakeCommentService.GetAllComments(1);

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Comment>>(result);
        }

        [Fact]
        public async void GetCommentById_HasComment_ReturnsCommentDto()
        {
            //Arrange
            _fakeCommentRepository.Setup(service => service.GetCommentById(1))
                .ReturnsAsync(new Comment());

            // Act
            var result = await _fakeCommentService.GetCommentById(1);

            // Assert
            Assert.IsType<Comment>(result);
        }

        [Fact]
        public async void CreateComment_CompleteComment_ReturnsCommentDto()
        {
            //Arrange
            var comment = new CommentCreationDto
            {
                Content = "sean"
            };

            var commentModel = new Comment
            {
                Id = 1,
                Content = "sean",
                PostId = 1
            };

            _fakeMapper.Setup(x => x.Map<Comment>(comment)).Returns(commentModel);
            _fakeCommentRepository.Setup(service => service.CreateComment(commentModel))
                .ReturnsAsync(1);

            // Act
            var result = await _fakeCommentService.CreateComment(1, comment);

            //Assert
            Assert.Equal(1, result.Id);
            Assert.Equal("sean", result.Content);
            Assert.Equal(1, result.PostId);
        }

        [Fact]
        public async void UpdateComment_CompleteComment_ReturnsCommentDto()
        {
            //Arrange
            var comment = new CommentUpdationDto
            {
                Content = "sean"
            };

            var commentModel = new Comment
            {
                Id = 1,
                Content = "sean",
                PostId = 1
            };

            _fakeMapper.Setup(x => x.Map<Comment>(comment)).Returns(commentModel);

            _fakeCommentRepository.Setup(service => service.UpdateComment(commentModel))
                .ReturnsAsync(1);

            // Act
            var result = await _fakeCommentService.UpdateComment(1, 1, comment);

            //Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async void DeleteComment_HasComment_ReturnsCommentDto()
        {
            //Arrange
            _fakeCommentRepository.Setup(service => service.DeleteComment(1, 1))
                .ReturnsAsync(1);

            // Act
            var result = await _fakeCommentService.DeleteComment(1, 1);

            //Assert
            Assert.Equal(1, result);
        }
    }
}
