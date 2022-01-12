using GoodNoodle.Application.ViewModel.Star;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodNoodle.Application.Interfaces;

public interface IStarService
{
    public Task<List<StarViewModel>> GetAllStarsAsync();

    public Task<List<StarViewModel>> GetAllStarsByGroupIdAsync(Guid groupId);

    public Task<List<StarViewModel>> GetAllStarsByUserIdAsync(Guid userId);

    public Task<Guid> CreateStarAsync(StarCreateViewModel viewModel);

    public Task UpdateStarAsync(Guid id, StarUpdateViewModel viewModel);

    public Task RemoveStarAsync(Guid id);
}
