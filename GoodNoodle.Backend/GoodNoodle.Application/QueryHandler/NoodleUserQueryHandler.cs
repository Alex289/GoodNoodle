using GoodNoodle.Application.Queries.NoodleUser;
using GoodNoodle.Application.ViewModel.NoodleUser;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNoodle.Application.QueryHandler;

public class NoodleUserQueryHandler : QueryHandler,
    IRequestHandler<GetAllNoodleUsersQuery, List<NoodleUserViewModel>>,
    IRequestHandler<GetNoodleUserWithIdQuery, NoodleUserViewModel>,
    IRequestHandler<GetNoodleUserWithNameQuery, NoodleUserViewModel>,
    IRequestHandler<GetNoodleUserWithGroupQuery, NoodleUserViewModel>,
    IRequestHandler<GetNoodleUserWithStatusQuery, List<NoodleUserViewModel>>,
    IRequestHandler<SearchNoodleUserWithNameQuery, List<NoodleUserViewModel>>
{
    private readonly INoodleUserRepository _noodleUserRepository;

    public NoodleUserQueryHandler(INoodleUserRepository noodleUserRepository, IMediator bus) : base(bus)
    {
        _noodleUserRepository = noodleUserRepository;
    }

    public async Task<List<NoodleUserViewModel>> Handle(GetAllNoodleUsersQuery request, CancellationToken cancellationToken)
    {
        List<NoodleUserViewModel> noodleUserListQuery = await _noodleUserRepository.GetAll()
            .Select(item => new NoodleUserViewModel
            {
                Id = item.Id,
                Email = item.Email,
                FullName = item.FullName,
                Status = item.Status,
                Role = item.Role
            })
            .ToListAsync(cancellationToken);

        return noodleUserListQuery;
    }

    public async Task<NoodleUserViewModel> Handle(GetNoodleUserWithIdQuery request, CancellationToken cancellationToken)
    {
        var noodleUserQuery = await _noodleUserRepository.GetByIdAsync(request.Id);

        if (noodleUserQuery == null)
        {
            await NotifyErrorAsync(DomainErrorCodes.NotFound, $"User {request.Id} not found");
            return new NoodleUserViewModel();
        }

        return new NoodleUserViewModel
        {
            Id = noodleUserQuery.Id,
            Email = noodleUserQuery.Email,
            FullName = noodleUserQuery.FullName,
            Status = noodleUserQuery.Status,
            Role = noodleUserQuery.Role
        };
    }

    public async Task<NoodleUserViewModel> Handle(GetNoodleUserWithNameQuery request, CancellationToken cancellationToken)
    {
        var noodleUserQuery = await _noodleUserRepository.GetByNameAsync(request.Name);

        if (noodleUserQuery == null)
        {
            await NotifyErrorAsync(DomainErrorCodes.NotFound, $"User {request.Name} not found");
            return new NoodleUserViewModel();
        }

        return new NoodleUserViewModel
        {
            Id = noodleUserQuery.Id,
            Email = noodleUserQuery.Email,
            FullName = noodleUserQuery.FullName,
            Status = noodleUserQuery.Status,
            Role = noodleUserQuery.Role
        };
    }

    public async Task<NoodleUserViewModel> Handle(GetNoodleUserWithGroupQuery request, CancellationToken cancellationToken)
    {
        var noodleUserQuery = await _noodleUserRepository.GetByGroupAsync(request.GroupId);

        if (noodleUserQuery == null)
        {
            await NotifyErrorAsync(DomainErrorCodes.NotFound, $"No user in group {request.GroupId} found");
        }

        return new NoodleUserViewModel
        {
            Id = noodleUserQuery.Id,
            Email = noodleUserQuery.Email,
            FullName = noodleUserQuery.FullName,
            Status = noodleUserQuery.Status,
            Role = noodleUserQuery.Role
        };
    }

    public async Task<List<NoodleUserViewModel>> Handle(GetNoodleUserWithStatusQuery request, CancellationToken cancellationToken)
    {
        var noodleUserListQuery = await _noodleUserRepository.GetAll()
            .Where(x => x.Status == request.Status)
            .Select(item => new NoodleUserViewModel
            {
                Id = item.Id,
                Email = item.Email,
                FullName = item.FullName,
                Status = item.Status,
                Role = item.Role
            }).ToListAsync(cancellationToken);

        return noodleUserListQuery;
    }

    public async Task<List<NoodleUserViewModel>> Handle(SearchNoodleUserWithNameQuery request, CancellationToken cancellationToken)
    {
        var noodleUserListQuery = await _noodleUserRepository.GetAll()
            .Where(x => x.FullName.Contains(request.Name))
            .Select(item => new NoodleUserViewModel
            {
                Id = item.Id,
                Email = item.Email,
                FullName = item.FullName,
                Status = item.Status
            }).ToListAsync(cancellationToken);

        return noodleUserListQuery;
    }
}
