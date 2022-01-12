using MediatR;
using System;

namespace GoodNoodle.Domain.Events.Invitations;
public class DeletedInvitationEvent : INotification
{
    public Guid Id { get; set; }

    public DeletedInvitationEvent(Guid id)
    {
        Id = id;
    }
}
