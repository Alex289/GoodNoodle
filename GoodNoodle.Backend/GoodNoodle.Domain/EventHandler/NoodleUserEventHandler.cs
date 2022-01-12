using GoodNoodle.Domain.Events.NoodleUser;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNoodle.Domain.EventHandler;

public class NoodleUserEventHandler :
    INotificationHandler<NoodleUserCreatedEvent>,
    INotificationHandler<NoodleUserUpdatedEvent>,
    INotificationHandler<NoodleUserChangedPasswordEvent>,
    INotificationHandler<NoodleUserDeletedEvent>
{
    public Task Handle(NoodleUserCreatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(NoodleUserUpdatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(NoodleUserChangedPasswordEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(NoodleUserDeletedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
