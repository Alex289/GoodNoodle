using GoodNoodle.Application.Interfaces;
using GoodNoodle.Application.Queries.Invitations;
using GoodNoodle.Application.Queries.NoodleGroup;
using GoodNoodle.Application.Queries.NoodleUser;
using GoodNoodle.Application.Queries.Star;
using GoodNoodle.Application.Queries.UserInGroup;
using GoodNoodle.Application.QueryHandler;
using GoodNoodle.Application.Services;
using GoodNoodle.Application.ViewModel.Invitations;
using GoodNoodle.Application.ViewModel.NoodleGroup;
using GoodNoodle.Application.ViewModel.NoodleUser;
using GoodNoodle.Application.ViewModel.Star;
using GoodNoodle.Application.ViewModel.UserInGroup;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace GoodNoodle.Application.Extension;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // User
        services.AddScoped<INoodleUserService, NoodleUserService>();

        // Group
        services.AddScoped<INoodleGroupService, NoodleGroupService>();

        // UserInGroup
        services.AddScoped<IUserInGroupService, UserInGroupService>();

        // Star
        services.AddScoped<IStarService, StarService>();

        // Invitations
        services.AddScoped<IInvitationsService, InvitationsService>();

        return services;
    }

    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        // User
        services.AddScoped<IRequestHandler<GetAllNoodleUsersQuery, List<NoodleUserViewModel>>, NoodleUserQueryHandler>();
        services.AddScoped<IRequestHandler<GetNoodleUserWithIdQuery, NoodleUserViewModel>, NoodleUserQueryHandler>();
        services.AddScoped<IRequestHandler<GetNoodleUserWithNameQuery, NoodleUserViewModel>, NoodleUserQueryHandler>();
        services.AddScoped<IRequestHandler<GetNoodleUserWithGroupQuery, NoodleUserViewModel>, NoodleUserQueryHandler>();
        services.AddScoped<IRequestHandler<GetNoodleUserWithStatusQuery, List<NoodleUserViewModel>>, NoodleUserQueryHandler>();
        services.AddScoped<IRequestHandler<SearchNoodleUserWithNameQuery, List<NoodleUserViewModel>>, NoodleUserQueryHandler>();

        // Group
        services.AddScoped<IRequestHandler<GetAllNoodleGroupsQuery, List<NoodleGroupViewModel>>, NoodleGroupQueryHandler>();
        services.AddScoped<IRequestHandler<GetGroupByIdQuery, NoodleGroupViewModel>, NoodleGroupQueryHandler>();

        // UserInGroup
        services.AddScoped<IRequestHandler<GetAllUserInGroupsQuery, List<UserInGroupViewModel>>, UserInGroupQueryHandler>();
        services.AddScoped<IRequestHandler<GetUserInGroupsByGroupQuery, List<NoodleUserInGroupViewModel>>, UserInGroupQueryHandler>();
        services.AddScoped<IRequestHandler<GetUserInGroupsByUserQuery, List<NoodleGroupViewModel>>, UserInGroupQueryHandler>();
        services.AddScoped<IRequestHandler<GetJoinableGroupsQuery, List<NoodleGroupViewModel>>, UserInGroupQueryHandler>();

        // Star
        services.AddScoped<IRequestHandler<GetAllStarsQuery, List<StarViewModel>>, StarQueryHandler>();
        services.AddScoped<IRequestHandler<GetAllStarsByGroupIdQuery, List<StarViewModel>>, StarQueryHandler>();
        services.AddScoped<IRequestHandler<GetAllStarsByUserIdQuery, List<StarViewModel>>, StarQueryHandler>();

        // Invitations
        services.AddScoped<IRequestHandler<GetInvitationsByGroupQuery, List<InvitationsViewModel>>, InvitationsQueryHandler>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // User
        services.AddScoped<INoodleUserRepository, NoodleUserRepository>();

        // Group
        services.AddScoped<INoodleGroupRepository, NoodleGroupRepository>();

        // UserInGroup
        services.AddScoped<IUserInGroupRepository, UserInGroupRepository>();

        // Star
        services.AddScoped<IStarRepository, StarRepository>();

        // Invitations
        services.AddScoped<IInvitationsRepository, InvitationsRepository>();

        return services;
    }
}
