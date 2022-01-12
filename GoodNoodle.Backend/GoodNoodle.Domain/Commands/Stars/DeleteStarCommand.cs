using System;

namespace GoodNoodle.Domain.Commands.Stars;

public class DeleteStarCommand : Command
{
    public Guid Id { get; set; }

    public DeleteStarCommand(Guid id)
    {
        Id = id;
    }
}
