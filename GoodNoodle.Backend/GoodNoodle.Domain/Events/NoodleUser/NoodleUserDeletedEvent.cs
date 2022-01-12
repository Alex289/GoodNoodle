using MediatR;
using System;

namespace GoodNoodle.Domain.Events.NoodleUser;

public class NoodleUserDeletedEvent : INotification
{
    public Guid Id { get; set; }

    public NoodleUserDeletedEvent(Guid id)
    {
        Id = id;
    }
}
