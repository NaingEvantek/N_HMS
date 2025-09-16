using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using N_HMS.Controllers;
using N_HMS.Interfaces;
using N_HMS.Models;
using Newtonsoft.Json.Linq;
using static N_HMS.PayLoad.PayLoadModel;

namespace N_HMS.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UserController(_mockService.Object);
        }

        [Fact]
        public async Task CreateUser_Should_Return_Ok_When_Success()
        {
            // Arrange
            var req = new UserCreateRequest { UserName = "testuser", Password = "Password123", RoleId = 1 };
            _mockService.Setup(s => s.CreateUserAsync(req.UserName, req.Password, req.RoleId))
                        .ReturnsAsync(new User_Info { Id = 1, User_Name = req.UserName });

            // Act
            var result = await _controller.CreateUser(req) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);

            var json = JObject.FromObject(result.Value);
            int userId = json["userId"]!.Value<int>();
            userId.Should().Be(1);
        }

        [Fact]
        public async Task CreateUser_Should_Return_BadRequest_When_Exception()
        {
            // Arrange
            var req = new UserCreateRequest { UserName = "testuser", Password = "Password123", RoleId = 1 };
            _mockService.Setup(s => s.CreateUserAsync(req.UserName, req.Password, req.RoleId))
                        .ThrowsAsync(new Exception("Username already exists"));

            // Act
            var result = await _controller.CreateUser(req) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(400);
            var json = JObject.FromObject(result.Value);
            var error = json["error"]!.Value<string>();
            error.Should().Be("Username already exists");
        }

        [Fact]
        public async Task ListUsers_Should_Return_User_List()
        {
            // Arrange
            var users = new List<User_Info>
            {
                new User_Info { Id = 1, User_Name = "user1", IsActive = true, Role = new Role_Info { Name = "Admin" }, Created_Date = DateTime.UtcNow },
                new User_Info { Id = 2, User_Name = "user2", IsActive = false, Role = new Role_Info { Name = "User" }, Created_Date = DateTime.UtcNow }
            };

            _mockService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _controller.ListUsers() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
            var value = result.Value as IEnumerable<dynamic>;
            value.Should().HaveCount(2);
        }
    }
}
