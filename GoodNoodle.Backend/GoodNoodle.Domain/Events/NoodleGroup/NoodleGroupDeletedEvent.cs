using MediatR;
using System;

namespace GoodNoodle.Domain.Events.NoodleGroup;

public class NoodleGroupDeletedEvent : INotification
{
    public Guid NoodleGroupId { get; set; }

    public NoodleGroupDeletedEvent(Guid noodleGroupId)
    {
        NoodleGroupId = noodleGroupId;
    }
}
