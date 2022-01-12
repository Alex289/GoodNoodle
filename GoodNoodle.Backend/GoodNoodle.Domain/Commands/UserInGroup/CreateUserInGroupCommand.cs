using System;

namespace GoodNoodle.Domain.Commands.UserInGroup;

public class CreateUserInGroupCommand : Command
{
    public Guid Id;
    public Guid UserInGroupId { get; set; }

    public CreateUserInGroupCommand(Guid id, Guid userInGroupId)
    {
        Id = id;
        UserInGroupId = userInGroupId;
    }
}
