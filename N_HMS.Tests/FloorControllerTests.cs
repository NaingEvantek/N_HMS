using System;
using System.Collections.Generic;
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
    public class FloorControllerTests
    {
        private readonly Mock<IFloorService> _mockService;
        private readonly FloorController _controller;

        public FloorControllerTests()
        {
            _mockService = new Mock<IFloorService>();
            _controller = new FloorController(_mockService.Object);
        }

        [Fact]
        public async Task CreateFloor_Should_Return_Ok_When_Success()
        {
            // Arrange
            var req = new FloorCreateRequest { FloorName = "First Floor" };
            _mockService.Setup(s => s.CreateFloorAsync(req.FloorName))
                        .ReturnsAsync(new Floor_Info { Id = 1, Name = req.FloorName });

            // Act
            var result = await _controller.CreateFloor(req) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);

            var json = JObject.FromObject(result.Value);
            var message = json["message"]!.Value<string>();
            message.Should().Be("Floor created successfully");
            var floorId = json["floorId"]!.Value<int>();
            floorId.Should().Be(1);
        }

        [Fact]
        public async Task CreateFloor_Should_Return_BadRequest_When_Exception()
        {
            // Arrange
            var req = new FloorCreateRequest { FloorName = "First Floor" };
            _mockService.Setup(s => s.CreateFloorAsync(req.FloorName))
                        .ThrowsAsync(new Exception("Floor already exists"));

            // Act
            var result = await _controller.CreateFloor(req) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(400);

            var json = JObject.FromObject(result.Value);
            var error = json["error"]!.Value<string>();
            error.Should().Be("Floor already exists");
        }

        [Fact]
        public async Task UpdateFloor_Should_Return_Ok_When_Success()
        {
            // Arrange
            var req = new FloorUpdateRequest { Id = 1, FloorName = "Updated Floor" };
            _mockService.Setup(s => s.UpdateFloorAsync(req.Id, req.FloorName))
                        .ReturnsAsync(new Floor_Info { Id = req.Id, Name = req.FloorName });

            // Act
            var result = await _controller.UpdateFloor(req) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);

            var json = JObject.FromObject(result.Value);
            var message = json["message"]!.Value<string>();
            message.Should().Be("Floor updated successfully");
        }

        [Fact]
        public async Task UpdateFloor_Should_Return_NotFound_When_Floor_Null()
        {
            // Arrange
            var req = new FloorUpdateRequest { Id = 1, FloorName = "Updated Floor" };
            _mockService.Setup(s => s.UpdateFloorAsync(req.Id, req.FloorName))
                        .ReturnsAsync((Floor_Info?)null);

            // Act
            var result = await _controller.UpdateFloor(req) as NotFoundObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(404);

            var json = JObject.FromObject(result.Value);
            var error = json["error"]!.Value<string>();
            error.Should().Be("Floor not found");
        }

        [Fact]
        public async Task ListUsers_Should_Return_Ok_With_Floor_List()
        {
            // Arrange
            var floors = new List<Floor_Info>
            {
                new Floor_Info { Id = 1, Name = "First Floor", Modified_Date = DateTime.UtcNow },
                new Floor_Info { Id = 2, Name = "Second Floor", Modified_Date = DateTime.UtcNow }
            };
            _mockService.Setup(s => s.GetAllFloorsAsync())
                        .ReturnsAsync(floors);

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
