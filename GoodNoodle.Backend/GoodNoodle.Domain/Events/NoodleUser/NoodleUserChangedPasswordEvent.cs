using MediatR;
using System;

namespace GoodNoodle.Domain.Events.NoodleUser;

public class NoodleUserChangedPasswordEvent : INotification
{
    public Guid Id { get; set; }

    public NoodleUserChangedPasswordEvent(Guid id)
    {
        Id = id;
    }
}
