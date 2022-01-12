using System;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Domain.Entities;
public class Invitations : Entity
{
    public Invitations(Guid Id) : base(Id)
    {
    }

    public Guid NoodleUserId { get; set; }
    public Guid NoodleGroupId { get; set; }
    public GroupRole Role { get; set; }
}
