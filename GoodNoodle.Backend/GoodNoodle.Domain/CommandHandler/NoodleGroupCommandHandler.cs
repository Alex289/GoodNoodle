using GoodNoodle.Domain.Commands.NoodleGroup;
using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Events.NoodleGroup;
using GoodNoodle.Domain.Events.UserInGroup;
using GoodNoodle.Domain.Interfaces;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Domain.Notifications;
using GoodNoodle.Domain.Validations.NoodleGroup;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Domain.CommandHandler;

public class NoodleGroupCommandHandler : CommandHandler,
    IRequestHandler<CreateNoodleGroupCommand>,
    IRequestHandler<UpdateNoodleGroupCommand>,
    IRequestHandler<DeleteNoodleGroupCommand>

{
    private readonly IMediator _bus;
    private readonly INoodleGroupRepository _noodleGroupRepository;
    private readonly IUserInGroupRepository _userInGroupRepository;
    private readonly IUser _user;

    public NoodleGroupCommandHandler(IUnitOfWork uow,
        IMediator bus,
        INoodleGroupRepository noodleGroupRepository,
        IUserInGroupRepository userInGroupRepository,
        INotificationHandler<DomainNotification> notifications,
        IUser userAccessor)
        : base(bus, uow, notifications, userInGroupRepository, userAccessor)
    {
        _bus = bus;
        _noodleGroupRepository = noodleGroupRepository;
        _userInGroupRepository = userInGroupRepository;
        _user = userAccessor;
    }

    public async Task<Unit> Handle(CreateNoodleGroupCommand request, CancellationToken cancellationToken)
    {
        if (!isAccepted())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserHasNoPermissions, "User must have permissions");
            return Unit.Value;
        }

        var validator = new ValidateCreateNoodleGroup();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            await NotifyErrorsAsync(validationResult);
            return Unit.Value;
        }

        var existingNoodleGroup = await _noodleGroupRepository.GetByIdAsync(request.Id);

        if (existingNoodleGroup != null)
        {
            await NotifyErrorAsync(DomainErrorCodes.GroupAlreadyExist, $"Group {existingNoodleGroup.Id} already exists");
            return Unit.Value;
        }

        var noodleGroup = new NoodleGroup(request.Id)
        {
            Name = request.Name,
            Image = request.Image
        };

        _noodleGroupRepository.Add(noodleGroup);

        // Add user to group
        var newUserInGroupId = Guid.NewGuid();
        var userInGroup = new UserInGroup(newUserInGroupId)
        {
            NoodleGroupId = request.Id,
            NoodleUserId = CurrentUserId,
            Role = GroupRole.Teacher
        };

        _userInGroupRepository.Add(userInGroup);

        if (await CommitAsync())
        {
            await _bus.Publish(new UserInGroupCreatedEvent(newUserInGroupId), cancellationToken);
            await _bus.Publish(new NoodleGroupCreatedEvent(request.Id), cancellationToken);
        }

        return Unit.Value;
    }

    public async Task<Unit> Handle(UpdateNoodleGroupCommand request, CancellationToken cancellationToken)
    {
        if (!isAccepted())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserHasNoPermissions, "User must have permissions");
            return Unit.Value;
        }

        var validator = new ValidateUpdateNoodleGroup();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            await NotifyErrorsAsync(validationResult);
            return Unit.Value;
        }

        if (!IsTeacherInGroup(request.Id).Result && !IsAdmin())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserIsNotAuthorized, $"No permission to update group {request.Id}");
            return Unit.Value;
        }

        var noodleGroup = await _noodleGroupRepository.GetByIdAsync(request.Id);

        if (noodleGroup == null)
        {
            await NotifyErrorAsync(DomainErrorCodes.NotFound, $"Group {request.Id} could not be found");
            return Unit.Value;
        }

        noodleGroup.Name = request.Name;
        noodleGroup.Image = request.Image;

        _noodleGroupRepository.Update(noodleGroup);

        if (await CommitAsync())
        {
            await _bus.Publish(new NoodleGroupUpdatedEvent(request.Id), cancellationToken);
        }

        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteNoodleGroupCommand request, CancellationToken cancellationToken)
    {
        if (!isAccepted())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserHasNoPermissions, "User must have permissions");
            return Unit.Value;
        }

        var validator = new ValidateDeleteNoodleGroup();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            await NotifyErrorsAsync(validationResult);
            return Unit.Value;
        }

        if (!IsTeacherInGroup(request.Id).Result && !IsAdmin())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserIsNotAuthorized, $"No permission to delete group {request.Id}");
            return Unit.Value;
        }

        var noodleGroup = await _noodleGroupRepository.GetByIdAsync(request.Id);

        if (noodleGroup == null)
        {
            await NotifyErrorAsync(DomainErrorCodes.NotFound, $"Group {request.Id} could not be found");
            return Unit.Value;
        }

        // Delete all items from user in groups too
        var usersInGroups = await _userInGroupRepository.GetByGroupAsync(request.Id);

        foreach (var currentUserInGroup in usersInGroups)
        {
            await _userInGroupRepository.RemoveAsync(currentUserInGroup.Id);
        }

        await _noodleGroupRepository.RemoveAsync(request.Id);

        if (await CommitAsync())
        {
            foreach (var userInGroup in usersInGroups)
            {
                await _bus.Publish(new UserInGroupDeletedByGroupEvent(request.Id, userInGroup.Id), cancellationToken);
            }
            await _bus.Publish(new NoodleGroupDeletedEvent(request.Id), cancellationToken);
        }

        return Unit.Value;
    }
}
