using GoodNoodle.Domain.Commands.UserInGroup;
using GoodNoodle.Domain.EmailTemplates;
using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Events.Invitations;
using GoodNoodle.Domain.Events.Star;
using GoodNoodle.Domain.Events.UserInGroup;
using GoodNoodle.Domain.Interfaces;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Domain.Notifications;
using GoodNoodle.Domain.Settings;
using GoodNoodle.Domain.Validations.UserInGroups;
using MailKit.Net.Smtp;
using MediatR;
using MimeKit;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNoodle.Domain.CommandHandler;

public class UserInGroupCommandHandler : CommandHandler,
    IRequestHandler<CreateUserInGroupCommand>,
    IRequestHandler<InviteUserCommand>,
    IRequestHandler<UpdateUserInGroupCommand>,
    IRequestHandler<DeleteUserInGroupCommand>
{
    private readonly IUserInGroupRepository _userInGroupRepository;
    private readonly IInvitationsRepository _invitationsRepository;
    private readonly IStarRepository _starRepository;
    private readonly IMediator _bus;
    private readonly MailSettings _mailSettings;
    private readonly HostingSettings _hostingSettings;

    public UserInGroupCommandHandler(
        IUnitOfWork uow,
        IMediator bus,
        INotificationHandler<DomainNotification> notifications,
        IUserInGroupRepository userInGroupRepository,
        IInvitationsRepository invitationsRepository,
        IStarRepository starRepository,
        IUser userAccessor,
        MailSettings mailSettings,
        HostingSettings hostingSettings)
        : base(bus, uow, notifications, userInGroupRepository, userAccessor)
    {
        _userInGroupRepository = userInGroupRepository;
        _invitationsRepository = invitationsRepository;
        _starRepository = starRepository;
        _bus = bus;
        _mailSettings = mailSettings;
        _hostingSettings = hostingSettings;
    }

    public async Task<Unit> Handle(CreateUserInGroupCommand request, CancellationToken cancellationToken)
    {
        if (!isAccepted())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserHasNoPermissions, "User must have permissions");
            return Unit.Value;
        }

        var validator = new CreateUserInGroupCommandValidation();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            await NotifyErrorsAsync(validationResult);
            return Unit.Value;
        }

        var invitation = await _invitationsRepository.GetByIdAsync(request.Id);

        if (invitation == null & !IsSelf(invitation != null ? invitation.NoodleUserId : Guid.NewGuid()))
        {
            await NotifyErrorAsync(DomainErrorCodes.UserIsNotAuthorized, $"No permission to create user in group");
            return Unit.Value;
        }

        var userInGroupByUser = await _userInGroupRepository.GetByUserAsync(invitation.NoodleUserId);
        UserInGroup existingUserInGroup = null;

        if (userInGroupByUser != null)
        {
            existingUserInGroup = userInGroupByUser.Where(x => x.NoodleGroupId == invitation.NoodleGroupId).FirstOrDefault();
        }

        if (existingUserInGroup != null)
        {
            await NotifyErrorAsync(DomainErrorCodes.UserInGroupAlreadyExists, $"User {existingUserInGroup.NoodleUserId} already exists in group {existingUserInGroup.NoodleGroupId}");
            return Unit.Value;
        }

        var userInGroup = new UserInGroup(request.UserInGroupId)
        {
            NoodleGroupId = invitation.NoodleGroupId,
            NoodleUserId = invitation.NoodleUserId,
            Role = invitation.Role
        };

        _userInGroupRepository.Add(userInGroup);
        await _invitationsRepository.RemoveAsync(request.Id);

        if (await CommitAsync())
        {
            await _bus.Publish(new UserInGroupCreatedEvent(request.UserInGroupId), cancellationToken);
            await _bus.Publish(new DeletedInvitationEvent(request.Id), cancellationToken);
        }

        return Unit.Value;
    }

    public async Task<Unit> Handle(InviteUserCommand request, CancellationToken cancellationToken)
    {
        var validator = new InviteUserCommandValidation();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            await NotifyErrorsAsync(validationResult);
            return Unit.Value;
        }

        if (!IsTeacherInGroup(request.GroupId).Result && !IsAdmin())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserIsNotAuthorized, $"No permission to invite user in group");
            return Unit.Value;
        }

        var newInvitationId = Guid.NewGuid();
        _invitationsRepository.Add(new Invitations(newInvitationId)
        {
            NoodleUserId = request.Id,
            NoodleGroupId = request.GroupId,
            Role = request.Role
        });

        if (await CommitAsync())
        {
            await _bus.Publish(new CreatedInvitationEvent(newInvitationId), cancellationToken);
        }

        var inviteLink = $"{_hostingSettings.Domain}/join/{newInvitationId}";

        MimeMessage message = new MimeMessage();

        MailboxAddress from = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
        message.From.Add(from);

        MailboxAddress to = new MailboxAddress(request.FullName, request.Email);
        message.To.Add(to);

        message.Subject = "Invitation to group";

        message.Body = new TextPart("html")
        {
            Text = InviteToGroup.content
                .Replace("@GROUP_NAME", request.GroupName)
                .Replace("@INVITE_LINK", inviteLink)
        };

        SmtpClient client = new SmtpClient();
        client.Connect(_mailSettings.Host, _mailSettings.Port, true);
        client.Authenticate(_mailSettings.Mail, _mailSettings.Password);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
        client.Dispose();

        return Unit.Value;
    }

    public async Task<Unit> Handle(UpdateUserInGroupCommand request, CancellationToken cancellationToken)
    {
        if (!isAccepted())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserHasNoPermissions, "User must have permissions");
            return Unit.Value;
        }

        var validator = new UpdateUserInGroupCommandValidation();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            await NotifyErrorsAsync(validationResult);
            return Unit.Value;
        }

        var userInGroup = await _userInGroupRepository.GetByIdAsync(request.Id);

        if (userInGroup == null)
        {
            await NotifyErrorAsync(DomainErrorCodes.NotFound, $"User {request.NoodleUserId} does not exist in group {request.NoodleGroupId}");
            return Unit.Value;
        }

        if (!IsSelf(userInGroup.NoodleUserId) && !IsTeacherInGroup(userInGroup.NoodleGroupId).Result && !IsAdmin())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserIsNotAuthorized, $"No permission to update user in group {request.Id}");
            return Unit.Value;
        }

        userInGroup.NoodleGroupId = request.NoodleGroupId;
        userInGroup.NoodleUserId = request.NoodleUserId;
        userInGroup.Role = request.Role;

        _userInGroupRepository.Update(userInGroup);

        if (await CommitAsync())
        {
            await _bus.Publish(new UserInGroupUpdatedEvent(userInGroup.Id), cancellationToken);
        }

        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteUserInGroupCommand request, CancellationToken cancellationToken)
    {
        if (!isAccepted())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserHasNoPermissions, "User must have permissions");
            return Unit.Value;
        }

        var validator = new DeleteUserInGroupCommandValidation();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            await NotifyErrorsAsync(validationResult);
            return Unit.Value;
        }

        var userInGroup = await _userInGroupRepository.GetByIdAsync(request.Id);

        if (userInGroup == null)
        {
            await NotifyErrorAsync(DomainErrorCodes.NotFound, $"User in group {request.Id} does not exist");
            return Unit.Value;
        }

        if (!IsSelf(userInGroup.NoodleUserId) && !IsTeacherInGroup(userInGroup.NoodleGroupId).Result && !IsAdmin())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserIsNotAuthorized, $"No permission to delete user in group {request.Id}");
            return Unit.Value;
        }

        await _userInGroupRepository.RemoveAsync(request.Id);

        // Delete stars of users in this group too
        var starsOfUserInGroup = await _starRepository.GetByUserAsync(userInGroup.NoodleUserId);
        if (starsOfUserInGroup != null)
        {
            foreach (var star in starsOfUserInGroup)
            {
                await _starRepository.RemoveAsync(star.Id);
            }
        }

        if (await CommitAsync())
        {
            if (starsOfUserInGroup != null)
            {
                foreach (var star in starsOfUserInGroup)
                {
                    await _bus.Publish(new DeletedStarByUserInGroupEvent(star.Id, request.Id));
                }
            }

            await _bus.Publish(new UserInGroupDeletedEvent(request.Id), cancellationToken);
        }

        return Unit.Value;
    }
}
