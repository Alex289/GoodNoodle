using System;

namespace GoodNoodle.Domain.Commands.Stars;

public class CreateStarCommand : Command
{
    public Guid Id { get; set; }

    public Guid NoodleUserId { get; set; }
    public Guid NoodleGroupId { get; set; }
    public string Reason { get; set; }

    public CreateStarCommand(Guid id, Guid noodleUserId, Guid noodleGroupId, string reason)
    {
        Id = id;
        NoodleUserId = noodleUserId;
        NoodleGroupId = noodleGroupId;
        Reason = reason;
    }
}
