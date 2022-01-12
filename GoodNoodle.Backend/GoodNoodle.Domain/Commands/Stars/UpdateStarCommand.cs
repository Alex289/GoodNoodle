using System;

namespace GoodNoodle.Domain.Commands.Stars;

public class UpdateStarCommand : Command
{
    public Guid Id { get; set; }
    public string Reason { get; set; }

    public UpdateStarCommand(Guid id, string reason)
    {
        Id = id;
        Reason = reason;
    }
}
