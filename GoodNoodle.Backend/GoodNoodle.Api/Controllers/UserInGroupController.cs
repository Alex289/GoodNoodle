using GoodNoodle.Application.Interfaces;
using GoodNoodle.Application.ViewModel.UserInGroup;
using GoodNoodle.Domain.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GoodNoodle.Api.Controllers;

[Authorize]
[Route("api/group")]
[ApiController]
public class UserInGroupController : ApiController
{
    private readonly IUserInGroupService _userInGroupService;

    public UserInGroupController(IUserInGroupService userInGroupService,
        INotificationHandler<DomainNotification> notifications) : base(notifications)
    {
        _userInGroupService = userInGroupService;
    }

    // GET: api/group/user/all
    [HttpGet("user/all")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userInGroupService.GetAllUserInGroupsAsync();
        return CreateResponse(result);
    }

    //POST: api/group/invite
    [HttpPost("invite")]
    public async Task<IActionResult> InviteUser(InviteUserViewModel viewModel)
    {
        await _userInGroupService.InviteUser(viewModel);
        return CreateResponse();
    }

    //POST: api/group/join
    [HttpPost("join")]
    public async Task<IActionResult> CreateUserInGroup(CreateUserInGroupViewModel viewModel)
    {
        var userInGroupId = await _userInGroupService.CreateUserInGroupAsync(viewModel);

        var response = new
        {
            Id = userInGroupId
        };

        return CreateResponse($"api/user/{userInGroupId}", response);
    }

    //PUT: api/group/user/update/GUID
    [HttpPut("user/update/{id}")]
    public async Task<IActionResult> UpdateUserInGroup(Guid id, UserInGroupViewModel viewModel)
    {
        await _userInGroupService.UpdateUserInGroupAsync(id, viewModel);
        return CreateResponse();
    }

    //DELETE: api/group/user/leave/GUID
    [HttpDelete("user/leave/{id}")]
    public async Task<IActionResult> RemoveUserInGroup(Guid id)
    {
        await _userInGroupService.DeleteUserInGroupAsync(id);
        return CreateResponse();
    }
}
