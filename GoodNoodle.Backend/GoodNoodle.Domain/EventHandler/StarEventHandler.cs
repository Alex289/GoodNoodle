using GoodNoodle.Domain.Events.Star;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNoodle.Domain.EventHandler;

public class StarEventHandler :
    INotificationHandler<CreatedStarEvent>,
    INotificationHandler<UpdatedStarEvent>,
    INotificationHandler<DeletedStarEvent>,
    INotificationHandler<DeletedStarByUserInGroupEvent>

{
    public Task Handle(CreatedStarEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(UpdatedStarEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(DeletedStarEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(DeletedStarByUserInGroupEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
