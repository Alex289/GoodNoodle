using MediatR;
using System;

namespace GoodNoodle.Domain.Events.UserInGroup;

public class UserInGroupDeletedEvent : INotification
{
    public Guid Id { get; set; }

    public UserInGroupDeletedEvent(Guid id)
    {
        Id = id;
    }
}
