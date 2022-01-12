using GoodNoodle.Domain.CommandHandler;
using GoodNoodle.Domain.Commands;
using GoodNoodle.Domain.Commands.NoodleGroup;
using GoodNoodle.Domain.Commands.NoodleUser;
using GoodNoodle.Domain.Commands.Stars;
using GoodNoodle.Domain.Commands.UserInGroup;
using GoodNoodle.Domain.EventHandler;
using GoodNoodle.Domain.Events.Invitations;
using GoodNoodle.Domain.Events.NoodleGroup;
using GoodNoodle.Domain.Events.NoodleUser;
using GoodNoodle.Domain.Events.Star;
using GoodNoodle.Domain.Events.UserInGroup;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GoodNoodle.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRequestHandlers(this IServiceCollection services)
    {
        // Group
        services.AddScoped<IRequestHandler<CreateNoodleGroupCommand, Unit>, NoodleGroupCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateNoodleGroupCommand, Unit>, NoodleGroupCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteNoodleGroupCommand, Unit>, NoodleGroupCommandHandler>();

        // User
        services.AddScoped<IRequestHandler<CreateNoodleUserCommand, Unit>, NoodleUserCommandHandler>();
        services.AddScoped<IRequestHandler<LoginNoodleUserCommand, string>, NoodleUserCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateNoodleUserCommand, Unit>, NoodleUserCommandHandler>();
        services.AddScoped<IRequestHandler<ChangePasswordCommand, Unit>, NoodleUserCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteNoodleUserCommand, Unit>, NoodleUserCommandHandler>();

        // User in group
        services.AddScoped<IRequestHandler<CreateUserInGroupCommand, Unit>, UserInGroupCommandHandler>();
        services.AddScoped<IRequestHandler<InviteUserCommand, Unit>, UserInGroupCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateUserInGroupCommand, Unit>, UserInGroupCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteUserInGroupCommand, Unit>, UserInGroupCommandHandler>();

        // Star
        services.AddScoped<IRequestHandler<CreateStarCommand, Unit>, StarCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateStarCommand, Unit>, StarCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteStarCommand, Unit>, StarCommandHandler>();

        return services;
    }

    public static IServiceCollection AddEventHandlers(this IServiceCollection services)
    {
        // Group
        services.AddScoped<INotificationHandler<NoodleGroupCreatedEvent>, NoodleGroupEventHandler>();
        services.AddScoped<INotificationHandler<NoodleGroupUpdatedEvent>, NoodleGroupEventHandler>();
        services.AddScoped<INotificationHandler<NoodleGroupDeletedEvent>, NoodleGroupEventHandler>();

        // User
        services.AddScoped<INotificationHandler<NoodleUserCreatedEvent>, NoodleUserEventHandler>();
        services.AddScoped<INotificationHandler<NoodleUserUpdatedEvent>, NoodleUserEventHandler>();
        services.AddScoped<INotificationHandler<NoodleUserChangedPasswordEvent>, NoodleUserEventHandler>();
        services.AddScoped<INotificationHandler<NoodleUserDeletedEvent>, NoodleUserEventHandler>();

        // User in group
        services.AddScoped<INotificationHandler<UserInGroupCreatedEvent>, UserInGroupEventHandler>();
        services.AddScoped<INotificationHandler<UserInGroupUpdatedEvent>, UserInGroupEventHandler>();
        services.AddScoped<INotificationHandler<UserInGroupDeletedEvent>, UserInGroupEventHandler>();
        services.AddScoped<INotificationHandler<UserInGroupDeletedByGroupEvent>, UserInGroupEventHandler>();
        services.AddScoped<INotificationHandler<UserInGroupDeletedByUserEvent>, UserInGroupEventHandler>();

        // Star
        services.AddScoped<INotificationHandler<CreatedStarEvent>, StarEventHandler>();
        services.AddScoped<INotificationHandler<UpdatedStarEvent>, StarEventHandler>();
        services.AddScoped<INotificationHandler<DeletedStarEvent>, StarEventHandler>();
        services.AddScoped<INotificationHandler<DeletedStarByUserInGroupEvent>, StarEventHandler>();

        // Invitations
        services.AddScoped<INotificationHandler<CreatedInvitationEvent>, InvitationsEventHandler>();
        services.AddScoped<INotificationHandler<DeletedInvitationEvent>, InvitationsEventHandler>();

        return services;
    }
}
