using GoodNoodle.Domain.Events.NoodleGroup;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNoodle.Domain.EventHandler;

class NoodleGroupEventHandler : INotificationHandler<NoodleGroupCreatedEvent>,
    INotificationHandler<NoodleGroupUpdatedEvent>,
    INotificationHandler<NoodleGroupDeletedEvent>
{
    public Task Handle(NoodleGroupCreatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(NoodleGroupUpdatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;

    }

    public Task Handle(NoodleGroupDeletedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
