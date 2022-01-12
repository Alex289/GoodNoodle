using MediatR;

namespace GoodNoodle.Domain.Commands;

public class LoginNoodleUserCommand : IRequest<string>
{
    public string Email { get; set; }
    public string Password { get; set; }

    public LoginNoodleUserCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
