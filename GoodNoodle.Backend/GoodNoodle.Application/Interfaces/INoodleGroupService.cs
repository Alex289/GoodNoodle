using GoodNoodle.Application.ViewModel.NoodleGroup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodNoodle.Application.Interfaces;

public interface INoodleGroupService
{
    public Task<List<NoodleGroupViewModel>> GetAllGroupsAsync();
    public Task<NoodleGroupViewModel> GetGroupByIdAsync(Guid groupId);
    public Task<Guid> CreateNoodleGroup(NoodleGroupCreateViewModel viewModel);
    public Task UpdateNoodleGroup(Guid id, NoodleGroupUpdateViewModel noodlegroup);
    public Task DeleteNoodleGroup(Guid id);
}
