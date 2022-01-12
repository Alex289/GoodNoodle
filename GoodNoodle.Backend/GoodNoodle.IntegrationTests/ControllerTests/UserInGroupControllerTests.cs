using FluentAssertions;
using GoodNoodle.Api;
using GoodNoodle.Application.ViewModel.Invitations;
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
public class UserInGroupControllerTests
    : IClassFixture<GoodNoodleFactory<Startup>>, IClassFixture<TestUserGroupFixture>
{
    private readonly HttpClient _client;
    private readonly TestUserGroupFixture _fixture;

    public UserInGroupControllerTests(GoodNoodleFactory<Startup> factory, TestUserGroupFixture fixture)
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
    public async Task ShouldInviteUser()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        _fixture.InvitedGroupId = Guid.NewGuid();

        var viewModel = new InviteUserViewModel()
        {
            Id = Guid.Parse("5D3F674E-BA9B-48EA-9C34-861EA84E7B44"),
            FullName = "Max Mustermann",
            Email = "max@mustermann.com",
            Role = GroupRole.Teacher,
            GroupId = _fixture.InvitedGroupId,
            GroupName = "test name"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/group/invite", viewModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact, Priority(2)]
    public async Task ShouldHaveInvitedUser()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/group/{_fixture.InvitedGroupId}/invitations");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsStringAsync();

        var deserialized = JsonConvert.DeserializeObject<ApiResponse<List<InvitationsViewModel>>>(message);

        deserialized.Success.Should().BeTrue();
        _fixture.InvitationId = deserialized.Data.FirstOrDefault().Id;
    }

    [Fact, Priority(4)]
    public async Task ShouldCreateUserInGroup()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        var viewModel = new CreateUserInGroupViewModel()
        {
            Id = _fixture.InvitationId
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/group/join", viewModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact, Priority(5)]
    public async Task ShouldNotCreateUserInGroupWithoutGroupId()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        var viewModel = new CreateUserInGroupViewModel()
        {
            Id = Guid.Empty,
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/group/join", viewModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var message = await response.Content.ReadAsStringAsync();

        var deserialized = JsonConvert.DeserializeObject<ApiResponse<UserInGroupViewModel>>(message);
        deserialized.Errors.First().Key.Should().Be("INVITATION_ID_MAY_MOT_BE_EMPTY");
        deserialized.Success.Should().BeFalse();
    }

    [Fact, Priority(10)]
    public async Task ShouldHaveCreatedUserInGroup()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync("/api/group/user/all");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsStringAsync();

        var deserialized = JsonConvert.DeserializeObject<ApiResponse<List<UserInGroupViewModel>>>(message);
        deserialized.Success.Should().BeTrue();
        deserialized.Data.Count.Should().Be(1);

        _fixture.UserInGroupViewModel = deserialized.Data.First();
    }

    [Fact, Priority(15)]
    public async Task ShouldUpdateUserInGroup()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        var viewModel = new UserInGroupViewModel()
        {
            NoodleGroupId = Guid.NewGuid(),
            NoodleUserId = Guid.NewGuid(),
            Role = GroupRole.Teacher
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/group/user/update/{_fixture.UserInGroupViewModel.Id}", viewModel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact, Priority(20)]
    public async Task ShouldHaveUpdatedUserInGroup()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/group/user/all");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var message = await response.Content.ReadAsStringAsync();

        var deserialized = JsonConvert.DeserializeObject<ApiResponse<List<UserInGroupViewModel>>>(message);
        deserialized.Success.Should().BeTrue();
        deserialized.Data.First().Role.Should().Be(GroupRole.Teacher);

        _fixture.UserInGroupViewModel = deserialized.Data.First();
    }

    [Fact, Priority(100)]
    public async Task ShouldRemoveUserInGroup()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.DeleteAsync($"/api/group/user/leave/{_fixture.UserInGroupViewModel.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact, Priority(101)]
    public async Task ShouldHaveNoUsersInGroups()
    {
        // Arrange
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _fixture.BearerToken);

        // Act
        var response = await _client.GetAsync($"/api/group/user/all");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var userInGroupList = await response.Content.ReadAsStringAsync();
        var deserialized = JsonConvert.DeserializeObject<ApiResponse<List<UserInGroupViewModel>>>(userInGroupList);

        deserialized.Success.Should().BeTrue();
        deserialized.Data.Count.Should().Be(0);
    }
}
