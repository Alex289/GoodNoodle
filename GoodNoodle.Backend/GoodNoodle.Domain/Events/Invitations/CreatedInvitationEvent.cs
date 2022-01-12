using MediatR;
using System;

namespace GoodNoodle.Domain.Events.Invitations;

public class CreatedInvitationEvent : INotification
{
    public Guid Id { get; set; }

    public CreatedInvitationEvent(Guid id)
    {
        Id = id;
    }
}
