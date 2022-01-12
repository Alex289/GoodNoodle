using System;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Application.ViewModel.NoodleUser;

public class NoodleUserViewModel
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public UserStatus Status { get; set; }
    public UserRole Role { get; set; }
}
