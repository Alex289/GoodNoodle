using GoodNoodle.Application.ViewModel.NoodleGroup;
using GoodNoodle.Application.ViewModel.NoodleUser;
using GoodNoodle.Application.ViewModel.UserInGroup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodNoodle.Application.Interfaces;

public interface IUserInGroupService
{
    public Task<List<UserInGroupViewModel>> GetAllUserInGroupsAsync();
    public Task<List<NoodleUserInGroupViewModel>> GetUserByGroupIdAsync(Guid groupId);
    public Task<List<NoodleGroupViewModel>> GetUserByUserIdAsync(Guid userId);
    public Task<List<NoodleGroupViewModel>> GetJoinableGroups(Guid userId);
    public Task<Guid> CreateUserInGroupAsync(CreateUserInGroupViewModel viewModel);
    public Task InviteUser(InviteUserViewModel viewModel);
    public Task UpdateUserInGroupAsync(Guid id, UserInGroupViewModel viewModel);
    public Task DeleteUserInGroupAsync(Guid id);
}
