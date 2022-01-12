using FluentAssertions;
using GoodNoodle.Api;
using GoodNoodle.Application.ViewModel.NoodleGroup;
using GoodNoodle.Application.ViewModel.NoodleUser;
using GoodNoodle.Application.ViewModel.UserInGroup;
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
public class NoodleGroupControllerTests
    : IClassFixture<GoodNoodleFactory<Startup>>, IClassFixture<TestGroupFixture>
{
    private readonly HttpClient _client;
    private readonly TestGroupFixture _fixture;

    public NoodleGroupControllerTests(GoodNoodleFactory<Startup> factory, TestGroupFixture fixture)
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
    public async Task ShouldCreateGroup()
    {
        var imageString = "7PuAiwHl9OzqyuB0AXTSSO+fBVg3iZGddNT8BCLeOzY=";

        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        var groupVM = new NoodleGroupCreateViewModel()
        {
            Image = imageString,
            Name = "Great Group"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/group", groupVM);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var location = response.Headers.Location.ToString();
        location.Substring(0, 10).Should().Be("api/group/");
        var guid = location.Substring(10);
        Guid.TryParse(guid, out _).Should().BeTrue();
    }

    [Fact, Priority(5)]
    public async Task ShouldNotCreateGroupWithoutName()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        var groupVM = new NoodleGroupCreateViewModel()
        {
            Image = "",
            Name = ""
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/group", groupVM);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var groupList = await response.Content.ReadAsStringAsync();

        var deserialized = JsonConvert.DeserializeObject<ApiResponse<List<NoodleGroupViewModel>>>(groupList);

        deserialized.Errors.First().Key.Should().Be("GROUP_NAME_MAY_NOT_BE_EMPTY");

        deserialized.Success.Should().BeFalse();
    }

    [Fact, Priority(10)]
    public async Task ShouldHaveCreatedGroup()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync("/api/group");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var groupList = await response.Content.ReadAsStringAsync();

        var deserialized = JsonConvert.DeserializeObject<ApiResponse<List<NoodleGroupViewModel>>>(groupList);
        deserialized.Data.First().Name.Should().Be("Great Group");

        _fixture.GroupViewModel = deserialized.Data.First();
        deserialized.Success.Should().BeTrue();
    }

    [Fact, Priority(11)]
    public async Task SetupMockData()
    {
        var guid = Guid.Parse("5D3F674E-BA9B-48EA-9C34-861EA84E7B44");

        _fixture.NoodleUserId = guid;

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        var response = await _client.GetAsync("/api/group/user/all");

        var userList = await response.Content.ReadAsStringAsync();

        var deserialized =
            JsonConvert.DeserializeObject<ApiResponse<List<UserInGroupViewModel>>>
            (userList);

        deserialized.Success.Should().BeTrue();

        _fixture.UserInGroupId = deserialized.Data.Find(x => x.NoodleUserId == _fixture.NoodleUserId).Id;
    }

    [Fact, Priority(13)]
    public async Task ShouldGetUser()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/group/{_fixture.GroupViewModel.Id}/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var userList = await response.Content.ReadAsStringAsync();

        var deserialized =
            JsonConvert.DeserializeObject<ApiResponse<List<NoodleUserViewModel>>>
            (userList);

        deserialized.Success.Should().BeTrue();
        deserialized.Data.First().Id.Should().Be(_fixture.NoodleUserId);

        // remove mock data so user in group tests dont fail
        await _client.DeleteAsync($"/api/group/user/leave/{_fixture.UserInGroupId}");
        await _client.DeleteAsync($"/api/user/{_fixture.NoodleUserId}");
    }

    [Fact, Priority(15)]
    public async Task ShouldUpdateGroup()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        var updateGroupVM = new NoodleGroupUpdateViewModel()
        {
            Image = "",
            Name = "Greatest Group"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/group/{_fixture.GroupViewModel.Id}", updateGroupVM);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact, Priority(20)]
    public async Task ShouldGetGroupById()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/group/{_fixture.GroupViewModel.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var group = await response.Content.ReadAsStringAsync();

        var deserialized = JsonConvert.DeserializeObject<ApiResponse<NoodleGroupViewModel>>(group);
        deserialized.Data.Name.Should().Be("Greatest Group");
        deserialized.Success.Should().BeTrue();
    }

    [Fact, Priority(25)]
    public async Task ShouldNoUpdateGroupWithEmptyName()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        var updateGroupVM = new NoodleGroupUpdateViewModel()
        {
            Image = "",
            Name = ""
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/group/{_fixture.GroupViewModel.Id}", updateGroupVM);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var groupList = await response.Content.ReadAsStringAsync();

        var deserialized = JsonConvert.DeserializeObject<ApiResponse<List<NoodleGroupViewModel>>>(groupList);

        deserialized.Errors.First().Key.Should().Be("GROUP_NAME_MAY_NOT_BE_EMPTY");

        deserialized.Success.Should().BeFalse();
    }

    [Fact, Priority(100)]
    public async Task ShouldRemoveGroup()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.DeleteAsync($"/api/group/{_fixture.GroupViewModel.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact, Priority(101)]
    public async Task ShouldHaveNoGroups()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync("/api/group");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var groupList = await response.Content.ReadAsStringAsync();
        var deserialized = JsonConvert.DeserializeObject<ApiResponse<List<NoodleGroupViewModel>>>(groupList);

        deserialized.Success.Should().BeTrue();
        deserialized.Data.Count.Should().Be(0);
    }
}
