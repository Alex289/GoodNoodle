using System;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Domain.Commands.UserInGroup;

public class InviteUserCommand : Command
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public GroupRole Role { get; set; }
    public Guid GroupId { get; set; }
    public string GroupName { get; set; }

    public InviteUserCommand(Guid id, string fullName, string email, GroupRole role, Guid groupId, string groupName)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        Role = role;
        GroupId = groupId;
        GroupName = groupName;
    }
}
