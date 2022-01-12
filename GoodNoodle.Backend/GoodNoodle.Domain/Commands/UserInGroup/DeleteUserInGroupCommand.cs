using System;

namespace GoodNoodle.Domain.Commands.UserInGroup;

public class DeleteUserInGroupCommand : Command
{
    public Guid Id { get; set; }

    public DeleteUserInGroupCommand(Guid id)
    {
        Id = id;
    }
}
