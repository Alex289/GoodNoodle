using System;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Application.ViewModel.UserInGroup;

public class InviteUserViewModel
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public GroupRole Role { get; set; }
    public Guid GroupId { get; set; }
    public string GroupName { get; set; }
}
