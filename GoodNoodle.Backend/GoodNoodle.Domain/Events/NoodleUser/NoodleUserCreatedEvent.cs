using MediatR;
using System;

namespace GoodNoodle.Domain.Events.NoodleUser;

public class NoodleUserCreatedEvent : INotification
{
    public Guid Id { get; set; }

    public NoodleUserCreatedEvent(Guid id)
    {
        Id = id;
    }
}
