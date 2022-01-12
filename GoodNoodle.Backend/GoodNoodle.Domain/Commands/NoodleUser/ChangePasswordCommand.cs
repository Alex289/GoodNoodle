namespace GoodNoodle.Domain.Commands.NoodleUser;

public class ChangePasswordCommand : Command
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }

    public ChangePasswordCommand(string oldPassword, string newPassword)
    {
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }
}
