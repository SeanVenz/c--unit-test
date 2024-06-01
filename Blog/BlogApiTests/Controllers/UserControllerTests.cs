using BlogApi.Controllers;
using BlogApi.Dtos;
using BlogApi.Models;
using BlogApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BlogApiTests.Controllers
{
    public class UserControllerTests
    {
        private readonly UserController _controller;
        private readonly Mock<IUserService> _fakeUserService;
        private readonly Mock<ILogger<UserController>> _logger;

        public UserControllerTests()
        {
            _fakeUserService = new Mock<IUserService>();
            _logger = new Mock<ILogger<UserController>>();
            _controller = new UserController(_fakeUserService.Object, _logger.Object);
        }

        //GetAllUser Tests
        [Fact]
        public async void GetAllUsers_HasUsers_ReturnsOk()
        {
            // Arrange
            _fakeUserService.Setup(service => service.GetAllUsers())
                .ReturnsAsync(new List<UserDto> { new UserDto() });

            // Act 
            var result = await _controller.GetAllUsers();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async void GetAllUsers_HasNoUsers_ReturnsNoContent()
        {
            // Arrange
            _fakeUserService.Setup(service => service.GetAllUsers())
                .ReturnsAsync(new List<UserDto>());

            // Act 
            var result = await _controller.GetAllUsers();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void GetAllUsers_Exception_ReturnsServerError()
        {
            // Arrange
            _fakeUserService.Setup(service => service.GetAllUsers())
                .Throws(new Exception());

            // Act 
            var result = await _controller.GetAllUsers();

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        //GetUserByID
        [Fact]
        public async void GetUserById_HasUser_ReturnsOk()
        {
            // Arrange
            _fakeUserService.Setup(service => service.GetUserById(10))
                .ReturnsAsync(new UserDto());

            // Act 
            var result = await _controller.GetUserById(10);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetUserById_HasNoUser_ReturnsNotFound()
        {
            //Arrange
            _fakeUserService.Setup(service => service.GetUserById(1))
                .ReturnsAsync(await Task.FromResult<UserDto>(null!));

            // Act 
            var result = await _controller.GetUserById(1);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetUserByID_Exception_ReturnsServerError()
        {
            // Arrange
            _fakeUserService.Setup(service => service.GetUserById(1))
                .Throws(new Exception());

            // Act 
            var result = await _controller.GetUserById(1);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        //CreateUser
        [Fact]
        public async void CreateUser_User_ReturnsCreated()
        {
            //Arrange
            var user = new UserCreationDto()
            {
                Username = "sean",
                Password = "sean",
                FirstName = "sean",
                LastName = "sean",
                EmailAddress = "sean@yahoo.com"
            };

            _fakeUserService.Setup(service => service.CreateUser(user))
                .ReturnsAsync(new User());

            //Act
            var result = await _controller.CreateUser(user);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(result);
        }

        //Does not work because it is not used in UserController Create Request

        //[Fact]
        //public async void CreateUser_NotAccepted_ReturnsBadRequest()
        //{
        //    // Arrange
        //    var missingUserName = new UserCreationDto()
        //    {
        //        FirstName = "sean",
        //        LastName = "sean",
        //        EmailAddress = "seanvenz.quijano@cit.edu",
        //        Password = "sean",
        //    };

        //    _controller.ModelState.AddModelError("Username", "Required");

        //    // Act
        //    var result = await _controller.CreateUser(missingUserName);

        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(result);
        //}

        [Fact]
        public async void CreateUser_ExistingUser_ReturnsConflict()
        {
            // Arrange
            var testUser = new UserCreationDto
            {
                Username = "sean",
                Password = "sean",
                FirstName = "Sean",
                LastName = "Sean",
                EmailAddress = "sean@gmail.com"
            };

            _fakeUserService.Setup(x => x.CheckIfUserExists(testUser.Username)).ReturnsAsync(409);

            // Act 
            var result = await _controller.CreateUser(testUser);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(409, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void CreateUser_Exception_ReturnsServerError()
        {
            // Arrange
            var user = new UserCreationDto()
            {
                Username = "sean",
                Password = "sean",
                FirstName = "sean",
                LastName = "sean",
                EmailAddress = "sean@yahoo.com"
            };

            _fakeUserService.Setup(service => service.CreateUser(user))
                .Throws(new Exception());

            // Act 
            var result = await _controller.CreateUser(user);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        //UpdateUser
        [Fact]
        public async void UpdateUser_UpdatedUser_ReturnsOk()
        {
            //Arrange
            var updatedUser = new UserUpdationDto
            {
                Username = "sean",
                FirstName = "sean",
                LastName = "sean",
                EmailAddress = "sean@yahoo.com"
            };

            _fakeUserService.Setup(service => service.UpdateUser(1, updatedUser))
                .ReturnsAsync(1);

            //Act
            var result = await _controller.UpdateUser(1, updatedUser);

            //Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async void UpdateUser_Conflict_ReturnsConflict()
        {
            //Arrange
            var updatedUser = new UserUpdationDto
            {
                Username = "sean",
                FirstName = "sean",
                LastName = "sean",
                EmailAddress = "sean@yahoo.com"
            };

            _fakeUserService.Setup(service => service.GetUserById(1))
                .ReturnsAsync(new UserDto());

            _fakeUserService.Setup(service => service.CheckIfUserExists(updatedUser.Username))
                .ReturnsAsync(409);

            //Act
            var result = await _controller.UpdateUser(1, updatedUser);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(409, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void UpdateUser_HasNoUser_ReturnsNotFound()
        {
            //Arrange
            var updatedUser = new UserUpdationDto
            {
                Username = "sean",
                FirstName = "sean",
                LastName = "sean",
                EmailAddress = "sean@yahoo.com"
            };

            _fakeUserService.Setup(service => service.UpdateUser(1, updatedUser))
                .ReturnsAsync(await Task.FromResult<int>(1));

            //Act
            var result = await _controller.UpdateUser(1, updatedUser);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void UpdateUser_Exception_ReturnsServerError()
        {
            //Arrange
            var updatedUser = new UserUpdationDto
            {
                Username = "sean",
                FirstName = "sean",
                LastName = "sean",
                EmailAddress = "sean@yahoo.com"
            };

            _fakeUserService.Setup(service => service.GetUserById(1))
                .ReturnsAsync(new UserDto());

            _fakeUserService.Setup(service => service.CheckIfUserExists(updatedUser.Username!))
                .ReturnsAsync(await Task.FromResult(0));

            _fakeUserService.Setup(service => service.UpdateUser(1, updatedUser))
                .Throws(new Exception());

            //Act
            var result = await _controller.UpdateUser(1, updatedUser);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        //DeleteUser
        [Fact]
        public async void DeleteUser_HasUser_ReturnsOk()
        {
            // Arrange
            _fakeUserService.Setup(service => service.DeleteUser(1))
                .ReturnsAsync(1);

            // Act 
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async void DeleteUser_HasNoUser_ReturnsNotFound()
        {
            // Arrange
            _fakeUserService.Setup(service => service.DeleteUser(1))
                .ReturnsAsync(await Task.FromResult<int>(1));

            // Act 
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, ((ObjectResult)result).StatusCode);

        }

        [Fact]
        public async void DeleteUser_Exception_ReturnsServerError()
        {
            //Arrange
            _fakeUserService.Setup(service => service.GetUserById(1))
                .ReturnsAsync(new UserDto());
            _fakeUserService.Setup(service => service.DeleteUser(1))
                .Throws(new Exception());

            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

        //GetUserPostsById
        [Fact]
        public async void GetUserPostsById_HasPost_ReturnsOk()
        {
            //Arrange
            _fakeUserService.Setup(service => service.GetUserPostsById(1))
                .ReturnsAsync(new UserPostDto());

            //Act
            var result = await _controller.GetUserPostsById(1);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetUserPostsById_HasNoPost_ReturnsNoContent()
        {
            //Arrange
            _fakeUserService.Setup(service => service.GetUserPostsById(1))
                .ReturnsAsync(await Task.FromResult<UserPostDto>(null!));

            //Act
            var result = await _controller.GetUserPostsById(1);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, ((ObjectResult)result).StatusCode); ;
        }

        [Fact]
        public async void GetUserPostsById_Exception_ReturnsServerError()
        {
            //Arrange
            _fakeUserService.Setup(service => service.GetUserPostsById(1))
                .Throws(new Exception());

            //Act
            var result = await _controller.GetUserPostsById(1);

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
        }

    }
}
