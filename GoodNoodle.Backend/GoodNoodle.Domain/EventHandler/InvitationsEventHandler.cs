using GoodNoodle.Domain.Events.Invitations;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNoodle.Domain.EventHandler;
public class InvitationsEventHandler : INotificationHandler<CreatedInvitationEvent>,
    INotificationHandler<DeletedInvitationEvent>
{
    public Task Handle(CreatedInvitationEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(DeletedInvitationEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
