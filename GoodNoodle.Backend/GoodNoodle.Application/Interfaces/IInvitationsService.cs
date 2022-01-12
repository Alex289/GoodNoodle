using GoodNoodle.Application.ViewModel.Invitations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodNoodle.Application.Interfaces;

public interface IInvitationsService
{
    public Task<List<InvitationsViewModel>> GetInvitationsByGroup(Guid groupId);
}
