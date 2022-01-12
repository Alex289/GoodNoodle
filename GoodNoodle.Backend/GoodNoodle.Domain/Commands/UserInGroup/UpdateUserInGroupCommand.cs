using System;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Domain.Commands.UserInGroup;

public class UpdateUserInGroupCommand : Command
{
    public Guid Id { get; set; }
    public Guid NoodleUserId { get; set; }
    public Guid NoodleGroupId { get; set; }
    public GroupRole Role { get; set; }

    public UpdateUserInGroupCommand(Guid id, Guid noodleUserId, Guid noodleGroupId, GroupRole role)
    {
        Id = id;
        NoodleUserId = noodleUserId;
        NoodleGroupId = noodleGroupId;
        Role = role;
    }
}
