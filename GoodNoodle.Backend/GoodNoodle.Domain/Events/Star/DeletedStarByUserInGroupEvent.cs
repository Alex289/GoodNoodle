using MediatR;
using System;

namespace GoodNoodle.Domain.Events.Star;
public class DeletedStarByUserInGroupEvent : INotification
{
    public Guid Id { get; set; }
    public Guid UserInGroupId { get; set; }

    public DeletedStarByUserInGroupEvent(Guid id, Guid userInGroupId)
    {
        Id = id;
        UserInGroupId = userInGroupId;
    }
}
