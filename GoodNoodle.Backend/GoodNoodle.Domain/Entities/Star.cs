using System;

namespace GoodNoodle.Domain.Entities;

public class Star : Entity
{
    public Star(Guid Id) : base(Id)
    {
    }

    public Guid NoodleUserId { get; set; }
    public Guid NoodleGroupId { get; set; }
    public string Reason { get; set; }
}
