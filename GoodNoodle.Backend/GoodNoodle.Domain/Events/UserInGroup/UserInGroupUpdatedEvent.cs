using MediatR;
using System;

namespace GoodNoodle.Domain.Events.UserInGroup;

public class UserInGroupUpdatedEvent : INotification
{
    public Guid Id { get; set; }

    public UserInGroupUpdatedEvent(Guid id)
    {
        Id = id;
    }
}
