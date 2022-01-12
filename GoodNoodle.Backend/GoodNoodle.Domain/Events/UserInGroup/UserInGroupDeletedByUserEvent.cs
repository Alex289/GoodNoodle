using MediatR;
using System;

namespace GoodNoodle.Domain.Events.UserInGroup;

public class UserInGroupDeletedByUserEvent : INotification
{
    public Guid Id { get; set; }
    public Guid NoodleUserId { get; set; }

    public UserInGroupDeletedByUserEvent(Guid noodleUserId, Guid id)
    {
        NoodleUserId = noodleUserId;
        Id = id;
    }
}
