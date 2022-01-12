using FluentAssertions;
using GoodNoodle.Api;
using GoodNoodle.Application.ViewModel.NoodleUser;
using GoodNoodle.Application.ViewModel.Star;
using GoodNoodle.IntegrationTests.Infrastructure;
using GoodNoodle.IntegrationTests.TestFixtures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace GoodNoodle.IntegrationTests.ControllerTests;

[Collection("GoodNoodle")]
[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class StarControllerTests
    : IClassFixture<GoodNoodleFactory<Startup>>, IClassFixture<TestStarFixture>
{
    private readonly HttpClient _client;
    private readonly TestStarFixture _fixture;

    public StarControllerTests(GoodNoodleFactory<Startup> factory, TestStarFixture fixture)
    {
        _client = factory.CreateClient();
        _fixture = fixture;
    }

    [Fact, Priority(0)]
    public async Task SetupAuthToken()
    {
        // Arrange
        var loginVM = new NoodleUserLoginViewModel()
        {
            Email = "max@mustermann.com",
            Password = "Password1#"
        };

        // Act
        var loginResponse = await _client.PostAsJsonAsync("/api/user/login", loginVM);

        // Assert
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await loginResponse.Content.ReadAsStringAsync();

        var deserialized =
            JsonConvert.DeserializeObject<ApiResponse<string>>(content);

        deserialized.Success.Should().BeTrue();

        _fixture.BearerToken = deserialized.Data;
    }

    [Fact, Priority(1)]
    public async Task ShouldCreateStar()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        var starViewModel = new StarCreateViewModel()
        {
            GroupId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Reason = "this is a reasonable reason"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/star", starViewModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var location = response.Headers.Location.ToString();
        location.Substring(0, 9).Should().Be("api/star/");

        var guid = location.Substring(9);
        Guid.TryParse(guid, out _).Should().BeTrue();
    }

    [Fact, Priority(5)]
    public async Task ShouldNotCreateStarWithoutReason()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        var star = new StarViewModel()
        {
            GroupId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Reason = ""
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/star", star);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var message = await response.Content.ReadAsStringAsync();

        var deserialized = JsonConvert.DeserializeObject<ApiResponse<StarViewModel>>(message);
        deserialized.Errors.First().Key.Should().Be("STAR_REASON_MAY_NOT_BE_EMPTY");
        deserialized.Success.Should().BeFalse();
    }

    [Fact, Priority(10)]
    public async Task ShouldHaveCreatedStar()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/star");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var starList = await response.Content.ReadAsStringAsync();

        var deserialized = JsonConvert.DeserializeObject<ApiResponse<List<StarViewModel>>>(starList);
        deserialized.Data.First().Reason.Should().Be("this is a reasonable reason");

        _fixture.StarViewModel = deserialized.Data.First();
        deserialized.Success.Should().BeTrue();
    }

    [Fact, Priority(15)]
    public async Task ShouldUpdateStar()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        var updateStarVM = new StarUpdateViewModel()
        {
            Reason = "this is a reasonable reason too"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/star/{_fixture.StarViewModel.Id}", updateStarVM);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact, Priority(20)]
    public async Task ShouldHaveUpdatedStar()
    {

        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/star/");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var starList = await response.Content.ReadAsStringAsync();

        var deserialized =
            JsonConvert.DeserializeObject<ApiResponse<List<StarViewModel>>>
            (starList);

        deserialized.Data.First().Reason.Should().Be("this is a reasonable reason too");
        deserialized.Success.Should().BeTrue();
    }

    [Fact, Priority(25)]
    public async Task ShouldNotUpdateStarWithEmptyReason()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        var updateStarVM = new StarUpdateViewModel()
        {
            Reason = ""
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/star/{_fixture.StarViewModel.Id}", updateStarVM);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var message = await response.Content.ReadAsStringAsync();

        var deserialized = JsonConvert.DeserializeObject<ApiResponse<StarViewModel>>(message);
        deserialized.Errors.First().Key.Should().Be("STAR_REASON_MAY_NOT_BE_EMPTY");
        deserialized.Success.Should().BeFalse();
    }

    [Fact, Priority(30)]
    public async Task ShouldGetStarByGroupId()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/star/group/{_fixture.StarViewModel.GroupId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact, Priority(35)]
    public async Task ShouldGetNoStarsWithoutGroupId()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/star/group/{Guid.Empty}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsStringAsync();

        var deserialized = JsonConvert.DeserializeObject<ApiResponse<List<StarViewModel>>>(message);
        deserialized.Data.Count.Should().Be(0);
    }

    [Fact, Priority(40)]
    public async Task ShouldGetStarByUserId()
    {
        // Arrange

        // Act
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        var response = await _client.GetAsync($"/api/star/user/{_fixture.StarViewModel.UserId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact, Priority(45)]
    public async Task ShouldGetNoStarsWithoutUserId()
    {
        // Arrange

        // Act
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        var response = await _client.GetAsync($"/api/star/user/{Guid.Empty}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsStringAsync();

        var deserialized = JsonConvert.DeserializeObject<ApiResponse<List<StarViewModel>>>(message);
        deserialized.Data.Count.Should().Be(0);
    }

    [Fact, Priority(100)]
    public async Task ShouldRemoveStar()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.DeleteAsync($"/api/star/{_fixture.StarViewModel.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact, Priority(101)]
    public async Task ShouldHaveNoStars()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/star");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var starList = await response.Content.ReadAsStringAsync();

        var deserialized = JsonConvert.DeserializeObject<ApiResponse<List<StarViewModel>>>(starList);

        deserialized.Success.Should().BeTrue();
        deserialized.Data.Count.Should().Be(0);
    }
}
