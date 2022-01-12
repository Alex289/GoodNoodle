using MediatR;
using System;

namespace GoodNoodle.Domain.Events.Star;

public class CreatedStarEvent : INotification
{
    public Guid Id { get; set; }

    public CreatedStarEvent(Guid id)
    {
        Id = id;
    }
}
