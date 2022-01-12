using FluentValidation.Results;
using GoodNoodle.Domain.Interfaces;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Domain.Notifications;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Domain.CommandHandler;

public class CommandHandler
{
    private readonly IMediator _bus;
    private readonly DomainNotificationHandler _notifications;
    private readonly IUserInGroupRepository _userInGroupRepository;
    private readonly IUnitOfWork _uow;
    private readonly IUser _user;

    public CommandHandler(
        IMediator bus,
        IUnitOfWork uow,
        INotificationHandler<DomainNotification> notifications,
        IUserInGroupRepository userInGroupRepository,
        IUser user)
    {
        _uow = uow;
        _notifications = (DomainNotificationHandler)notifications;
        _bus = bus;
        _userInGroupRepository = userInGroupRepository;
        _user = user;
    }

    public async Task<bool> CommitAsync()
    {
        if (_notifications.HasNotifications())
        {
            return false;
        }

        if (await _uow.CommitAsync())
        {
            return true;
        }

        await _bus.Publish(
            new DomainNotification(
                "Commit",
                "Problem saving data. Please try again."));
        return false;
    }

    protected async Task NotifyErrorAsync(string key, string value)
    {
        await _bus.Publish(
            new DomainNotification(
                key,
                value));
    }

    protected async Task NotifyErrorsAsync(ValidationResult message)
    {
        if (message == null)
        {
            return;
        }

        foreach (ValidationFailure error in message.Errors)
        {
            await _bus.Publish(new DomainNotification(error.ErrorCode, error.ErrorMessage));
        }
    }

    protected bool isAccepted()
    {
        return _user.Status == "Accepted";
    }

    protected bool IsAdmin()
    {
        return _user.Role == "Admin";
    }

    protected Guid CurrentUserId => _user.Id;

    protected bool IsSelf(Guid userId)
    {
        return CurrentUserId == userId;
    }

    protected async Task<bool> IsTeacherInGroup(Guid groupId)
    {
        var allGroupsOfUser = await _userInGroupRepository.GetByUserAsync(CurrentUserId);

        if (allGroupsOfUser == null)
        {
            return false;
        }

        var userInGroup = allGroupsOfUser.Where(x => x.NoodleGroupId == groupId).FirstOrDefault();

        if (userInGroup == null)
        {
            return false;
        }

        if (userInGroup.Role == GroupRole.Teacher)
        {
            return true;
        }

        return false;
    }
}
