using System;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Domain.Commands;

public class UpdateNoodleUserCommand : Command
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public UserStatus Status { get; set; }
    public UserRole Role { get; set; }

    public UpdateNoodleUserCommand(Guid id, string fullName, UserStatus status, string email, UserRole role)
    {
        Id = id;
        FullName = fullName;
        Status = status;
        Email = email;
        Role = role;
    }
}
