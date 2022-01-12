using GoodNoodle.Application.Interfaces;
using GoodNoodle.Application.ViewModel.Star;
using GoodNoodle.Domain.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GoodNoodle.Api.Controllers;

[Authorize]
[Route("api/star")]
[ApiController]
public class StarController : ApiController
{
    private readonly IStarService _starService;

    public StarController(IStarService starService, INotificationHandler<DomainNotification> notifications) : base(notifications)
    {
        _starService = starService;
    }

    // GET: api/star
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _starService.GetAllStarsAsync();
        return CreateResponse(result);
    }

    // GET: api/star/group/GUID
    [HttpGet("group/{groupId}")]
    public async Task<IActionResult> GetAllByGroupId(Guid groupId)
    {
        var result = await _starService.GetAllStarsByGroupIdAsync(groupId);
        return CreateResponse(result);
    }

    // GET: api/star/user/GUID
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetAllByUserId(Guid userId)
    {
        var result = await _starService.GetAllStarsByUserIdAsync(userId);
        return CreateResponse(result);
    }

    // POST: api/star/
    [HttpPost]
    public async Task<IActionResult> CreateStar(StarCreateViewModel viewModel)
    {
        var starId = await _starService.CreateStarAsync(viewModel);
        return CreateResponse($"api/star/{starId}", viewModel);
    }

    //PUT: api/star/GUID
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStar(Guid id, StarUpdateViewModel viewModel)
    {
        await _starService.UpdateStarAsync(id, viewModel);
        return CreateResponse();
    }

    //DELETE: api/star/GUID
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveStar(Guid id)
    {
        await _starService.RemoveStarAsync(id);
        return CreateResponse();
    }
}
