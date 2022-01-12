using GoodNoodle.Domain.Commands.Stars;
using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Events.Star;
using GoodNoodle.Domain.Interfaces;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Domain.Notifications;
using GoodNoodle.Domain.Validations.Star;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNoodle.Domain.CommandHandler;

public class StarCommandHandler : CommandHandler,
    IRequestHandler<CreateStarCommand>,
    IRequestHandler<UpdateStarCommand>,
    IRequestHandler<DeleteStarCommand>
{
    private readonly IStarRepository _starRepository;
    private readonly IMediator _bus;

    public StarCommandHandler(
        IUnitOfWork uow,
        IMediator bus,
        INotificationHandler<DomainNotification> notifications,
        IStarRepository starRepository,
        IUserInGroupRepository userInGroupRepository,
        IUser userAccessor)
        : base(bus, uow, notifications, userInGroupRepository, userAccessor)
    {
        _starRepository = starRepository;
        _bus = bus;
    }

    public async Task<Unit> Handle(CreateStarCommand request, CancellationToken cancellationToken)
    {
        if (!isAccepted())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserHasNoPermissions, "User must have permissions");
            return Unit.Value;
        }

        var validations = new ValidateCreateStar();
        var validationResult = validations.Validate(request);
        if (!validationResult.IsValid)
        {
            await NotifyErrorsAsync(validationResult);
            return Unit.Value;
        }

        if (!IsTeacherInGroup(request.NoodleGroupId).Result && !IsAdmin())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserIsNotAuthorized, "No permission to create star");
            return Unit.Value;
        }

        var checkStar = await _starRepository.GetByIdAsync(request.Id);

        if (checkStar != null)
        {
            await NotifyErrorAsync(DomainErrorCodes.StarIdAlreadyExists, $"Star {checkStar.Id} already exists");
            return Unit.Value;
        }

        var star = new Star(request.Id)
        {
            NoodleUserId = request.NoodleUserId,
            NoodleGroupId = request.NoodleGroupId,
            Reason = request.Reason
        };

        _starRepository.Add(star);

        if (await CommitAsync())
        {
            await _bus.Publish(new CreatedStarEvent(request.Id), cancellationToken);
        }

        return Unit.Value;
    }

    public async Task<Unit> Handle(UpdateStarCommand request, CancellationToken cancellationToken)
    {
        if (!isAccepted())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserHasNoPermissions, "User must have permissions");
            return Unit.Value;
        }

        var validations = new ValidateUpdateStar();
        var validationResult = validations.Validate(request);
        if (!validationResult.IsValid)
        {
            await NotifyErrorsAsync(validationResult);
            return Unit.Value;
        }

        var star = await _starRepository.GetByIdAsync(request.Id);

        if (star == null)
        {
            await NotifyErrorAsync(DomainErrorCodes.NotFound, $"Star {request.Id} does not exist");
            return Unit.Value;
        }

        if (!IsTeacherInGroup(star.NoodleGroupId).Result && !IsAdmin())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserIsNotAuthorized, $"No permission to update star {request.Id}");
            return Unit.Value;
        }

        star.Reason = request.Reason;

        _starRepository.Update(star);

        if (await CommitAsync())
        {
            await _bus.Publish(new UpdatedStarEvent(request.Id), cancellationToken);
        }

        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteStarCommand request, CancellationToken cancellationToken)
    {
        if (!isAccepted())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserHasNoPermissions, "User must have permissions");
            return Unit.Value;
        }

        var validations = new ValidateDeleteStar();
        var validationResult = validations.Validate(request);
        if (!validationResult.IsValid)
        {
            await NotifyErrorsAsync(validationResult);
            return Unit.Value;
        }

        var star = await _starRepository.GetByIdAsync(request.Id);

        if (star == null)
        {
            await NotifyErrorAsync(DomainErrorCodes.NotFound, $"Star {request.Id} does not exist");
            return Unit.Value;
        }

        if (!IsTeacherInGroup(star.NoodleGroupId).Result && !IsAdmin())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserIsNotAuthorized, $"No permission to delete star {request.Id}");
            return Unit.Value;
        }

        await _starRepository.RemoveAsync(request.Id);

        if (await CommitAsync())
        {
            await _bus.Publish(new DeletedStarEvent(request.Id), cancellationToken);
        }

        return Unit.Value;
    }
}
