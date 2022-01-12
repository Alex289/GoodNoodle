using System;

namespace GoodNoodle.Domain.Commands;

public class DeleteNoodleUserCommand : Command
{
    public Guid Id;

    public DeleteNoodleUserCommand(Guid id)
    {
        Id = id;
    }
}
