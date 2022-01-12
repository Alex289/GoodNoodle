using System;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Domain.Entities;

public class UserInGroup : Entity
{
    public UserInGroup(Guid Id) : base(Id)
    {
    }

    public Guid NoodleUserId { get; set; }
    public virtual NoodleUser NoodleUser { get; set; }
    public Guid NoodleGroupId { get; set; }
    public virtual NoodleGroup NoodleGroup { get; set; }
    public GroupRole Role { get; set; }
}
