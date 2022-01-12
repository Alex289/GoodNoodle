using GoodNoodle.Application.Interfaces;
using GoodNoodle.Application.ViewModel.NoodleGroup;
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
public class NoodleGroupController : ApiController
{
    private readonly INoodleGroupService _groupService;
    private readonly IUserInGroupService _userInGroupService;
    private readonly IInvitationsService _invitationsService;

    public NoodleGroupController(
        INoodleGroupService groupService,
        IUserInGroupService userInGroupService,
        IInvitationsService invitationsService,
        INotificationHandler<DomainNotification> notifications)
        : base(notifications)
    {
        _groupService = groupService;
        _userInGroupService = userInGroupService;
        _invitationsService = invitationsService;
    }

    // GET: api/group
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _groupService.GetAllGroupsAsync();
        return CreateResponse(result);
    }

    // GET: api/group/Id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _groupService.GetGroupByIdAsync(id);
        return CreateResponse(result);
    }

    // GET: api/group/groupId/users
    [HttpGet("{groupId}/users")]
    public async Task<IActionResult> GetUsersInGroupById(Guid groupId)
    {
        var result = await _userInGroupService.GetUserByGroupIdAsync(groupId);
        return CreateResponse(result);
    }

    // GET: api/group/joinable/GUID
    [HttpGet("joinable/{userId}")]
    public async Task<IActionResult> GetJoinableGroupsOfUser(Guid userId)
    {
        var result = await _userInGroupService.GetJoinableGroups(userId);
        return CreateResponse(result);
    }

    // GET: api/group/GUID/invitations
    [HttpGet("{groupId}/invitations")]
    public async Task<IActionResult> GetNoodleGroupInvitations(Guid groupId)
    {
        var result = await _invitationsService.GetInvitationsByGroup(groupId);
        return CreateResponse(result);
    }

    // POST: api/group
    [HttpPost]
    public async Task<IActionResult> CreateNoodleGroup(NoodleGroupCreateViewModel viewModel)
    {
        Guid groupId = await _groupService.CreateNoodleGroup(viewModel);
        var response = new NoodleGroupViewModel() { Id = groupId, Name = viewModel.Name, Image = viewModel.Image };
        return CreateResponse($"api/group/{response.Id}", response);
    }

    // PUT: api/group/Id
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNoodleGroup(Guid id, NoodleGroupUpdateViewModel viewModel)
    {
        await _groupService.UpdateNoodleGroup(id, viewModel);
        return CreateResponse();
    }

    // DELETE: api/group/Id
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveNoodleGroup(Guid id)
    {
        await _groupService.DeleteNoodleGroup(id);
        return CreateResponse();
    }
}
