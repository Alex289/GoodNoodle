using System;
using System.Collections.Generic;

namespace GoodNoodle.Domain.Entities;

public class NoodleGroup : Entity
{
    public NoodleGroup(Guid Id) : base(Id)
    {

    }

    public string Name { get; set; }
    public string Image { get; set; }
    public virtual List<UserInGroup> UserInGroups { get; set; }
}
