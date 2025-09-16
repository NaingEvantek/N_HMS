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
using N_HMS.DTO;
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
            var req = new QueryRequest { PageIndex = 1, PageSize = 2, SortBy = "username", IsDescending = false };
            var pagedResult = new PagedResult<UserDTO>
            {
                PageIndex = 1,
                PageSize = 2,
                TotalCount = 3,
                Items = new List<UserDTO>
                {
                    new UserDTO { Id = 1, User_Name = "John" ,Role_Name="Admin",Role_Id=1 ,IsActive=true},
                    new UserDTO { Id = 2, User_Name = "Jane" , Role_Name="User" ,Role_Id=2,IsActive=true}
                }
            };
            _mockService.Setup(s => s.GetAllUsersAsync(req.PageIndex, req.PageSize, req.SortBy, req.IsDescending))
                        .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.ListUsers(req) as OkObjectResult;


            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);

            var value = result.Value as PagedResult<UserDTO>;
            value.Should().NotBeNull();
            value!.Items.Should().HaveCount(2);
            value.TotalCount.Should().Be(3);
        }
    }
}
