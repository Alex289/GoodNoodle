using System;
using System.Collections.Generic;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Domain.Entities;

public class NoodleUser : Entity
{
    public NoodleUser(Guid id) : base(id)
    {
    }

    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserStatus Status { get; set; }
    public UserRole Role { get; set; }
    public virtual List<UserInGroup> UserInGroups { get; set; }
}
