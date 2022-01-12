using GoodNoodle.Domain.Events.UserInGroup;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNoodle.Domain.EventHandler;

public class UserInGroupEventHandler :
    INotificationHandler<UserInGroupCreatedEvent>,
    INotificationHandler<UserInGroupUpdatedEvent>,
    INotificationHandler<UserInGroupDeletedEvent>,
    INotificationHandler<UserInGroupDeletedByGroupEvent>,
    INotificationHandler<UserInGroupDeletedByUserEvent>
{
    public Task Handle(UserInGroupCreatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(UserInGroupUpdatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(UserInGroupDeletedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(UserInGroupDeletedByGroupEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(UserInGroupDeletedByUserEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
