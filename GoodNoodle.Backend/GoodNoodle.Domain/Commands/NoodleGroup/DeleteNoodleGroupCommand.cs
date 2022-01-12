using System;

namespace GoodNoodle.Domain.Commands.NoodleGroup;

public class DeleteNoodleGroupCommand : Command
{
    public Guid Id;

    public DeleteNoodleGroupCommand(Guid id)
    {
        Id = id;
    }
}
