using System;

namespace GoodNoodle.Domain.Interfaces;

public interface IUser
{
    public Guid Id { get; }
    public string Role { get; }
    public string Status { get; }
}
