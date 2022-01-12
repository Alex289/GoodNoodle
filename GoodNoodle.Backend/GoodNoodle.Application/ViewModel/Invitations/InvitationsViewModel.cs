using System;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Application.ViewModel.Invitations;
public class InvitationsViewModel
{
    public Guid Id { get; set; }
    public Guid NoodleGroupId { get; set; }
    public Guid NoodleUserId { get; set; }
    public GroupRole Role { get; set; }
}
