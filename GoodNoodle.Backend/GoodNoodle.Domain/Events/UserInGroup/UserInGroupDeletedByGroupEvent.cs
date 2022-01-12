using MediatR;
using System;

namespace GoodNoodle.Domain.Events.UserInGroup;

public class UserInGroupDeletedByGroupEvent : INotification
{
    public Guid Id { get; set; }
    public Guid NoodleGroupId { get; set; }

    public UserInGroupDeletedByGroupEvent(Guid noodleGroupId, Guid id)
    {
        NoodleGroupId = noodleGroupId;
        Id = id;
    }
}
