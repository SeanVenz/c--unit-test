using AutoMapper;
using BlogApi.Dtos;
using BlogApi.Mappings;
using BlogApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApiTests.Mappings
{
    public class UserMappingTests
    {
        private readonly IMapper _mapper;

        public UserMappingTests()
        {
            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile(new UserMappings()));

            mappingConfig.AssertConfigurationIsValid();

            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public void Map_ValidUserUpdationDto_ReturnUser()
        {
            // Arrange
            var dto = new UserUpdationDto
            {
                FirstName = "lorem",
                LastName = "ipsum",
                Username = "lipsum",
                EmailAddress = "lipsum@dolor.com"
            };

            var expectedUser = new User
            {
                Username = "lipsum",
                EmailAddress = "lipsum@dolor.com",
                FirstName = "Lorem",
                LastName = "Ipsum"
            };

            // Act
            var result = _mapper.Map<User>(dto);

            // Assert
            Assert.Equal(result.EmailAddress, expectedUser.EmailAddress);
            Assert.Equal(result.FirstName, expectedUser.FirstName);
            Assert.Equal(result.LastName, expectedUser.LastName);
            Assert.Equal(result.Username, expectedUser.Username);
        }

        [Fact]
        public void Map_ValidUseCreationDto_ReturnUser()
        {
            // Arrange
            var dto = new UserCreationDto
            {
                FirstName = "lorem",
                LastName = "ipsum",
                Username = "lipsum",
                EmailAddress = "lipsum@dolor.com",
                Password = "CCS"
            };

            var expectedUser = new User
            {
                Username = "lipsum",
                EmailAddress = "lipsum@dolor.com",
                FirstName = "Lorem",
                LastName = "Ipsum"
            };

            // Act
            var result = _mapper.Map<User>(dto);

            // Assert
            Assert.Equal(result.EmailAddress, expectedUser.EmailAddress);
            Assert.Equal(result.FirstName, expectedUser.FirstName);
            Assert.Equal(result.LastName, expectedUser.LastName);
            Assert.Equal(result.Username, expectedUser.Username);
        }
    }
}
