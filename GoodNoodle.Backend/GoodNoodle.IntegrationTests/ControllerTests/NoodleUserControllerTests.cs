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
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.IntegrationTests.ControllerTests;

[Collection("GoodNoodle")]
[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class NoodleUserControllerTests
    : IClassFixture<GoodNoodleFactory<Startup>>, IClassFixture<TestUserFixture>
{
    private readonly HttpClient _client;
    private readonly TestUserFixture _fixture;

    public NoodleUserControllerTests(GoodNoodleFactory<Startup> factory, TestUserFixture fixture)
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

    [Fact, Priority(0)]
    public async Task ShouldCreateUser()
    {
        // Arrange
        var userVM = new NoodleUserRegisterViewModel()
        {
            Email = "user@noodle.com",
            FullName = "Maxim Mustermann",
            Password = "Password123#"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/user/register", userVM);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var location = response.Headers.Location.ToString();
        location.Substring(0, 9).Should().Be("api/user/");
        var guid = location.Substring(9);
        Guid.TryParse(guid, out _).Should().BeTrue();

        var content = await response.Content.ReadAsStringAsync();

        var deserialized =
            JsonConvert.DeserializeObject<ApiResponse<string>>(content);

        deserialized.Success.Should().BeTrue();

        _fixture.NewCreatedUserToken = deserialized.Data;
    }

    [Fact, Priority(1)]
    public async Task ShouldLoginUser()
    {
        // Arrange
        var userVM = new NoodleUserLoginViewModel()
        {
            Email = "user@noodle.com",
            Password = "Password123#"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/user/login", userVM);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();

        var deserialized =
            JsonConvert.DeserializeObject<ApiResponse<string>>(content);

        deserialized.Success.Should().BeTrue();
    }

    [Fact, Priority(5)]
    public async Task ShouldNotCreateUserWithoutName()
    {

        // Arrange
        var userVM = new NoodleUserRegisterViewModel()
        {
            Email = "maxim@mustermann.com",
            FullName = "",
            Password = "Password123#"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/user/register", userVM);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var user = await response.Content.ReadAsStringAsync();

        var deserialized =
            JsonConvert.DeserializeObject<ApiResponse<List<NoodleUserViewModel>>>
            (user);

        deserialized.Errors.First().Key.Should().Be("USER_FULL_NAME_MAY_NOT_BE_EMPTY");
        deserialized.Success.Should().BeFalse();
    }

    [Fact, Priority(15)]
    public async Task ShouldHaveCreatedUser()
    {

        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/user");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var userList = await response.Content.ReadAsStringAsync();

        var deserialized =
            JsonConvert.DeserializeObject<ApiResponse<List<NoodleUserViewModel>>>
            (userList);

        _fixture.UserViewModel = deserialized.Data.First();
        deserialized.Data.First().FullName.Should().Be("Max Mustermann");
        deserialized.Success.Should().BeTrue();
    }

    [Fact, Priority(16)]
    public async Task SetupMockData()
    {

        // Create temp group
        // Arrange
        var groupVM = new NoodleGroupCreateViewModel()
        {
            Image = "",
            Name = "Great Group"
        };

        // Act
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        var response = await _client.PostAsJsonAsync("/api/group", groupVM);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();

        var location = response.Headers.Location.ToString();
        location.Substring(0, 10).Should().Be("api/group/");
        var guid = Guid.Parse(location.Substring(10));

        _fixture.GroupId = guid;

        // Get User in Group
        var userInGroupResponse = await _client.GetAsync("/api/group/user/all");

        var userList = await userInGroupResponse.Content.ReadAsStringAsync();

        var deserialized =
            JsonConvert.DeserializeObject<ApiResponse<List<UserInGroupViewModel>>>
            (userList);

        deserialized.Success.Should().BeTrue();

        _fixture.UserInGroupId = deserialized.Data.Find(x => x.NoodleGroupId == guid).Id;
    }

    [Fact, Priority(19)]
    public async Task ShouldGetGroup()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/user/{_fixture.UserViewModel.Id}/groups");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var userList = await response.Content.ReadAsStringAsync();

        var deserialized =
            JsonConvert.DeserializeObject<ApiResponse<List<NoodleGroupViewModel>>>
            (userList);

        deserialized.Success.Should().BeTrue();
        deserialized.Data.First().Id.Should().Be(_fixture.GroupId);

        // remove mock data so user in group tests dont fail
        await _client.DeleteAsync($"/api/group/user/leave/{_fixture.UserInGroupId}");
        await _client.DeleteAsync($"/api/group/{_fixture.GroupId}");
    }

    [Fact, Priority(20)]
    public async Task ShouldUpdateUser()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        var updateVM = new NoodleUserUpdateViewModel()
        {
            Email = "markus@mueller.com",
            FullName = "Markus Mueller",
            Status = UserStatus.Declined
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/user/{_fixture.UserViewModel.Id}", updateVM);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact, Priority(21)]
    public async Task ShouldChangePassword()
    {
        // Arrange
        var changePasswordVM = new NoodleUserChangePasswordViewModel()
        {
            NewPassword = "NewPassword1#",
            OldPassword = "Password123#"
        };

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.NewCreatedUserToken);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/user/change-password", changePasswordVM);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact, Priority(25)]
    public async Task ShouldHaveUpdatedUser()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/user/{_fixture.UserViewModel.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var userList = await response.Content.ReadAsStringAsync();

        var deserialized =
            JsonConvert.DeserializeObject<ApiResponse<NoodleUserViewModel>>
            (userList);

        deserialized.Data.FullName.Should().Be("Markus Mueller");
        deserialized.Data.Status.Should().Be(UserStatus.Declined);
        _fixture.UserViewModel = deserialized.Data; // update fixture to keep it consistent with in memory database
        deserialized.Success.Should().BeTrue();
    }

    [Fact, Priority(30)]
    public async Task ShouldGetUserById()
    {

        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/user/{_fixture.UserViewModel.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await response.Content.ReadAsStringAsync();

        var deserialized =
            JsonConvert.DeserializeObject<ApiResponse<NoodleUserViewModel>>
            (user);

        deserialized.Data.Should().BeEquivalentTo(_fixture.UserViewModel);

        deserialized.Data.FullName.Should().Be("Markus Mueller");
        deserialized.Success.Should().BeTrue();
    }

    [Fact, Priority(35)]
    public async Task ShouldGetUserByFirstName()
    {

        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/user/name/{_fixture.UserViewModel.FullName}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await response.Content.ReadAsStringAsync();

        var deserialized =
            JsonConvert.DeserializeObject<ApiResponse<NoodleUserViewModel>>
            (user);

        deserialized.Data.Should().BeEquivalentTo(_fixture.UserViewModel);

        deserialized.Data.FullName.Should().Be("Markus Mueller");
        deserialized.Success.Should().BeTrue();
    }

    [Fact, Priority(40)]
    public async Task ShouldGetUserByStatus()
    {

        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/user/status/2");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var userList = await response.Content.ReadAsStringAsync();

        var deserialized =
            JsonConvert.DeserializeObject<ApiResponse<List<NoodleUserViewModel>>>
            (userList);

        deserialized.Data.First().Should().BeEquivalentTo(_fixture.UserViewModel);

        deserialized.Data.First().Status.Should().Be(UserStatus.Declined);
        deserialized.Success.Should().BeTrue();
    }

    [Fact, Priority(100)]
    public async Task ShouldRemoveUser()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.DeleteAsync($"/api/user/{_fixture.UserViewModel.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact, Priority(101)]
    public async Task ShouldHaveNoUsers()
    {

        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/user");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var userList = await response.Content.ReadAsStringAsync();
        var deserialized = JsonConvert.DeserializeObject<ApiResponse<List<NoodleUserViewModel>>>(userList);

        deserialized.Success.Should().BeTrue();

        // Only initialized user from factory should be left
        deserialized.Data.Count.Should().Be(1);
    }
}
