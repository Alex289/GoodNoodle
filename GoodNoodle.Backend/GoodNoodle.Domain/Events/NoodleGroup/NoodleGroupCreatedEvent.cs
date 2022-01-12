using MediatR;
using System;

namespace GoodNoodle.Domain.Events.NoodleGroup;

public class NoodleGroupCreatedEvent : INotification
{
    public Guid NoodleGroupId { get; set; }

    public NoodleGroupCreatedEvent(Guid noodleGroupId)
    {
        NoodleGroupId = noodleGroupId;
    }
}
