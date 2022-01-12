using System;

namespace GoodNoodle.Domain.Commands;

public class CreateNoodleUserCommand : Command
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public CreateNoodleUserCommand(
        Guid id,
        string fullName,
        string password,
        string email)
    {
        Id = id;
        FullName = fullName;
        Password = password;
        Email = email;
    }
}
