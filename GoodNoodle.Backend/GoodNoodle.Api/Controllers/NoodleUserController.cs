using GoodNoodle.Application.Interfaces;
using GoodNoodle.Application.ViewModel.NoodleUser;
using GoodNoodle.Domain.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Api.Controllers;

[Route("api/user")]
[ApiController]
public class NoodleUserController : ApiController
{
    private readonly INoodleUserService _userService;
    private readonly IUserInGroupService _userInGroupService;

    public NoodleUserController(
        INoodleUserService userService,
        IUserInGroupService userInGroupService,
        INotificationHandler<DomainNotification> notifications)
        : base(notifications)
    {
        _userService = userService;
        _userInGroupService = userInGroupService;
    }

    // GET: api/user
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllNoodleUsersAsync();
        return CreateResponse(result);
    }

    //GET: api/user/GUID
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNoodleUserById(Guid id)
    {
        var result = await _userService.GetNoodleUserByIdAsync(id);
        return CreateResponse(result);
    }

    //GET: api/user/name/karl
    [Authorize]
    [HttpGet("name/{fullName}")]
    public async Task<IActionResult> GetNoodleUserByName(string fullName)
    {
        var result = await _userService.GetNoodleUserByNameAsync(fullName);
        return CreateResponse(result);
    }

    //GET: api/user/search/alex
    [Authorize]
    [HttpGet("search/{name}")]
    public async Task<IActionResult> SearchNoodleUserByName(string name)
    {
        var result = await _userService.SearchNoodleUserByNameAsync(name);
        return CreateResponse(result);
    }

    //GET: api/user/GUID/groups
    [Authorize]
    [HttpGet("{userId}/groups")]
    public async Task<IActionResult> GetNoodleGroupsByUser(Guid userId)
    {
        var result = await _userInGroupService.GetUserByUserIdAsync(userId);
        return CreateResponse(result);
    }

    //GET: api/user/status/accepted
    [Authorize]
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetNoodleUserByStatus(UserStatus status)
    {
        var result = await _userService.GetNoodleUserByStatusAsync(status);
        return CreateResponse(result);
    }

    //POST: api/user/register
    [HttpPost("register")]
    public async Task<IActionResult> CreateNoodleUser(NoodleUserRegisterViewModel viewModel)
    {
        var userModel = new NoodleUserCreateViewModel()
        {
            Email = viewModel.Email,
            Password = viewModel.Password,
            FullName = viewModel.FullName,
        };

        var token = await _userService.CreateNoodleUser(userModel);
        return CreateResponse($"api/user/{userModel.Id}", token);
    }

    //POST: api/user/login
    [HttpPost("login")]
    public async Task<IActionResult> NoodleUserLogin(NoodleUserLoginViewModel viewModel)
    {
        var token = await _userService.LoginNoodleUser(viewModel);
        return CreateResponse(token);
    }

    //PUT: api/user/GUID
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNoodleUser(Guid id, NoodleUserUpdateViewModel viewModel)
    {
        await _userService.UpdateNoodleUser(id, viewModel);
        return CreateResponse();
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(NoodleUserChangePasswordViewModel viewModel)
    {
        await _userService.ChangePassword(viewModel);
        return CreateResponse();
    }

    //DELETE: api/user/GUID
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveNoodleUser(Guid id)
    {
        await _userService.RemoveNoodleUser(id);
        return CreateResponse();
    }
}
