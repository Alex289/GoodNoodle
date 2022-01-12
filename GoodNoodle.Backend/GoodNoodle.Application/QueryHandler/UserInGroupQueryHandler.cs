using GoodNoodle.Application.Queries.UserInGroup;
using GoodNoodle.Application.ViewModel.NoodleGroup;
using GoodNoodle.Application.ViewModel.NoodleUser;
using GoodNoodle.Application.ViewModel.UserInGroup;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNoodle.Application.QueryHandler;

public class UserInGroupQueryHandler : QueryHandler,
    IRequestHandler<GetAllUserInGroupsQuery, List<UserInGroupViewModel>>,
    IRequestHandler<GetUserInGroupsByGroupQuery, List<NoodleUserInGroupViewModel>>,
    IRequestHandler<GetUserInGroupsByUserQuery, List<NoodleGroupViewModel>>,
    IRequestHandler<GetJoinableGroupsQuery, List<NoodleGroupViewModel>>
{
    private readonly IUserInGroupRepository _userInGroupRepository;
    private readonly INoodleUserRepository _noodleUserRepository;
    private readonly INoodleGroupRepository _noodleGroupRepository;

    public UserInGroupQueryHandler(
        IUserInGroupRepository userInGroupRepository,
        IMediator bus,
        INoodleUserRepository noodleUserRepository,
        INoodleGroupRepository noodleGroupRepository) : base(bus)
    {
        _userInGroupRepository = userInGroupRepository;
        _noodleUserRepository = noodleUserRepository;
        _noodleGroupRepository = noodleGroupRepository;
    }

    public async Task<List<UserInGroupViewModel>> Handle(GetAllUserInGroupsQuery request, CancellationToken cancellationToken)
    {
        var userInGroups = await _userInGroupRepository.GetAll()
            .Select(item => new UserInGroupViewModel
            {
                Id = item.Id,
                NoodleGroupId = item.NoodleGroupId,
                NoodleUserId = item.NoodleUserId,
                Role = item.Role,
            })
            .ToListAsync(cancellationToken);

        return userInGroups;
    }

    public async Task<List<NoodleUserInGroupViewModel>> Handle(GetUserInGroupsByGroupQuery request, CancellationToken cancellationToken)
    {
        List<NoodleUserInGroupViewModel> noodleUsers = new List<NoodleUserInGroupViewModel>();
        var noodleGroup = await _noodleGroupRepository.GetByIdAsync(request.GroupId);

        if (noodleGroup == null)
        {
            await NotifyErrorAsync(DomainErrorCodes.NotFound, $"Group ${request.GroupId} could not be found");
            return null;
        }

        foreach (var item in noodleGroup.UserInGroups)
        {
            noodleUsers.Add(new NoodleUserInGroupViewModel()
            {
                Id = item.NoodleUser.Id,
                Email = item.NoodleUser.Email,
                FullName = item.NoodleUser.FullName,
                Status = item.NoodleUser.Status,
                Role = item.NoodleUser.Role,
                GroupRole = item.Role
            });
        }

        return noodleUsers;
    }

    public async Task<List<NoodleGroupViewModel>> Handle(GetUserInGroupsByUserQuery request, CancellationToken cancellationToken)
    {
        List<NoodleGroupViewModel> noodleUsers = new List<NoodleGroupViewModel>();
        var noodleUser = await _noodleUserRepository.GetByIdAsync(request.UserId);

        foreach (var item in noodleUser.UserInGroups)
        {
            noodleUsers.Add(new NoodleGroupViewModel()
            {
                Id = item.NoodleGroup.Id,
                Name = item.NoodleGroup.Name,
                Image = item.NoodleGroup.Image
            });
        }

        return noodleUsers;
    }

    public async Task<List<NoodleGroupViewModel>> Handle(GetJoinableGroupsQuery request, CancellationToken cancellationToken)
    {
        var filteredGroups = await _noodleGroupRepository.GetAll()
            .Where(group => group.UserInGroups.All(uig => uig.NoodleUserId != request.UserId))
            .Select(ng => new NoodleGroupViewModel
            {
                Id = ng.Id,
                Name = ng.Name,
                Image = ng.Image
            })
            .ToListAsync(cancellationToken: cancellationToken);

        return filteredGroups;
    }
}
