using MediatR;
using System;

namespace GoodNoodle.Domain.Events.Star;

public class DeletedStarEvent : INotification
{
    public Guid Id { get; set; }

    public DeletedStarEvent(Guid id)
    {
        Id = id;
    }
}
