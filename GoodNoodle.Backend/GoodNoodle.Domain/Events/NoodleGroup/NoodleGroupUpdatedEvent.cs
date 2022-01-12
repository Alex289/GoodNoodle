using MediatR;
using System;

namespace GoodNoodle.Domain.Events.NoodleGroup;

public class NoodleGroupUpdatedEvent : INotification
{
    public Guid NoodleGroupId { get; set; }

    public NoodleGroupUpdatedEvent(Guid noodleGroupId)
    {
        NoodleGroupId = noodleGroupId;
    }
}
