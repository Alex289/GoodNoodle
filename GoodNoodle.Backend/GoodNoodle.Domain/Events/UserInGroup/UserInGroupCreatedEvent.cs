using MediatR;
using System;

namespace GoodNoodle.Domain.Events.UserInGroup;

public class UserInGroupCreatedEvent : INotification
{
    public Guid Id { get; set; }

    public UserInGroupCreatedEvent(Guid id)
    {
        Id = id;
    }
}
