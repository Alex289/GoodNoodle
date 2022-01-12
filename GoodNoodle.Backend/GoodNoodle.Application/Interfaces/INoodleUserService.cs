using GoodNoodle.Application.ViewModel.NoodleUser;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Application.Interfaces;

public interface INoodleUserService
{
    public Task<List<NoodleUserViewModel>> GetAllNoodleUsersAsync();
    public Task<NoodleUserViewModel> GetNoodleUserByIdAsync(Guid id);
    public Task<NoodleUserViewModel> GetNoodleUserByNameAsync(string name);
    public Task<NoodleUserViewModel> GetNoodleUserByGroupAsync(Guid groupId);
    public Task<List<NoodleUserViewModel>> GetNoodleUserByStatusAsync(UserStatus status);
    public Task<List<NoodleUserViewModel>> SearchNoodleUserByNameAsync(string name);

    public Task<string> CreateNoodleUser(NoodleUserCreateViewModel noodleUser);
    public Task<string> LoginNoodleUser(NoodleUserLoginViewModel noodleUser);
    public Task UpdateNoodleUser(Guid id, NoodleUserUpdateViewModel noodleUser);
    public Task ChangePassword(NoodleUserChangePasswordViewModel changePasswordViewModel);
    public Task RemoveNoodleUser(Guid id);
}
