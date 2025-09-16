using System;
using System.Collections.Generic;
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
    public class GuestControllerTests
    {
        private readonly Mock<IGuestService> _mockService;
        private readonly GuestController _controller;

        public GuestControllerTests()
        {
            _mockService = new Mock<IGuestService>();
            _controller = new GuestController(_mockService.Object);
        }

        [Fact]
        public async Task CreateGuest_Should_Return_Ok_When_Success()
        {
            // Arrange
            var req = new GuestCreateRequest { GuestName = "John", PassportNo = "P123", GenderId = 1 };
            _mockService.Setup(s => s.CreateGuestAsync(req.GuestName, req.PassportNo, req.GenderId))
                        .ReturnsAsync(new Guest_Info { Id = 1, Name = req.GuestName });

            // Act
            var result = await _controller.CreateGuest(req) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);

            var json = JObject.FromObject(result.Value);
            var message = json["message"]!.Value<string>();
            message.Should().Be("User created successfully");
        }

        [Fact]
        public async Task CreateGuest_Should_Return_BadRequest_When_Exception()
        {
            // Arrange
            var req = new GuestCreateRequest { GuestName = "John", PassportNo = "P123", GenderId = 1 };
            _mockService.Setup(s => s.CreateGuestAsync(req.GuestName, req.PassportNo, req.GenderId))
                        .ThrowsAsync(new Exception("Duplicate guest"));

            // Act
            var result = await _controller.CreateGuest(req) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(400);

            var json = JObject.FromObject(result.Value);
            var error = json["error"]!.Value<string>();
            error.Should().Be("Duplicate guest");
           
        }

        [Fact]
        public async Task UpdateGuest_Should_Return_Ok_When_Success()
        {
            // Arrange
            var req = new GuestUpdateRequest { Id = 1, GuestName = "John Updated", PassportNo = "P123", GenderId = 1 };
            _mockService.Setup(s => s.UpdateGuestAsync(req.Id, req.GuestName, req.PassportNo, req.GenderId))
                        .ReturnsAsync(new Guest_Info { Id = 1, Name = req.GuestName });

            // Act
            var result = await _controller.UpdateUser(req) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);

            var json = JObject.FromObject(result.Value);
            var message = json["message"]!.Value<string>();
            message.Should().Be("Guest updated successfully");

        }

        [Fact]
        public async Task UpdateGuest_Should_Return_NotFound_When_Guest_Null()
        {
            // Arrange
            var req = new GuestUpdateRequest { Id = 1, GuestName = "John Updated", PassportNo = "P123", GenderId = 1 };
            _mockService.Setup(s => s.UpdateGuestAsync(req.Id, req.GuestName, req.PassportNo, req.GenderId))
                        .ReturnsAsync((Guest_Info?)null);

            // Act
            var result = await _controller.UpdateUser(req) as NotFoundObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(404);

            var json = JObject.FromObject(result.Value);
            var error = json["error"]!.Value<string>();
            error.Should().Be("Guest not found");
        }

        [Fact]
        public async Task ListGuests_Should_Return_Ok_With_PagedResult()
        {
            // Arrange
            var req = new QueryRequest { PageIndex = 1, PageSize = 2, SortBy = "GuestName", IsDescending = false };
            var pagedResult = new PagedResult<GuestDTO>
            {
                PageIndex = 1,
                PageSize = 2,
                TotalCount = 3,
                Items = new List<GuestDTO>
                {
                    new GuestDTO { Id = 1, GuestName = "John" },
                    new GuestDTO { Id = 2, GuestName = "Jane" }
                }
            };
            _mockService.Setup(s => s.GetAllGuestsAsync(req))
                        .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.ListGuests(req) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);

            var value = result.Value as PagedResult<GuestDTO>;
            value.Should().NotBeNull();
            value!.Items.Should().HaveCount(2);
            value.TotalCount.Should().Be(3);
        }
    }
}
