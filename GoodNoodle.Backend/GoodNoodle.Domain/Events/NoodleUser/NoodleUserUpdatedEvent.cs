using MediatR;
using System;

namespace GoodNoodle.Domain.Events.NoodleUser;

public class NoodleUserUpdatedEvent : INotification
{
    public Guid Id { get; set; }

    public NoodleUserUpdatedEvent(Guid id)
    {
        Id = id;
    }
}
