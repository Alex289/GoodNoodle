using MediatR;
using System;

namespace GoodNoodle.Domain.Events.Star;

public class UpdatedStarEvent : INotification
{
    public Guid Id { get; set; }

    public UpdatedStarEvent(Guid id)
    {
        Id = id;
    }
}
