using GoodNoodle.Domain.Commands;
using GoodNoodle.Domain.Commands.NoodleUser;
using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Events.NoodleUser;
using GoodNoodle.Domain.Events.UserInGroup;
using GoodNoodle.Domain.Interfaces;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Domain.Notifications;
using GoodNoodle.Domain.Settings;
using GoodNoodle.Domain.Validations.NoodleUser;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GoodNoodle.Domain.Enums;
using BC = BCrypt.Net.BCrypt;

namespace GoodNoodle.Domain.CommandHandler;

public class NoodleUserCommandHandler : CommandHandler,
    IRequestHandler<CreateNoodleUserCommand>,
    IRequestHandler<LoginNoodleUserCommand, string>,
    IRequestHandler<UpdateNoodleUserCommand>,
    IRequestHandler<ChangePasswordCommand>,
    IRequestHandler<DeleteNoodleUserCommand>
{
    private const double EXPIRY_DURATION_MINUTES = 30;
    private readonly INoodleUserRepository _noodleUserRepository;
    private readonly IUserInGroupRepository _userInGroupRepository;
    private readonly IMediator _bus;
    private readonly TokenSettings _tokenSettings;

    public NoodleUserCommandHandler(IUnitOfWork uow,
        IMediator bus,
        INotificationHandler<DomainNotification> notifications,
        INoodleUserRepository noodleUserRepository,
        IUserInGroupRepository userInGroupRepository,
        TokenSettings tokenSettings,
        IUser userAccessor)
        : base(bus, uow, notifications, userInGroupRepository, userAccessor)
    {
        _noodleUserRepository = noodleUserRepository;
        _userInGroupRepository = userInGroupRepository;
        _bus = bus;
        _tokenSettings = tokenSettings;
    }

    public async Task<Unit> Handle(CreateNoodleUserCommand request, CancellationToken cancellationToken)
    {
        var validations = new ValidateCreateNoodleUser();
        var validationResult = validations.Validate(request);
        if (!validationResult.IsValid)
        {
            await NotifyErrorsAsync(validationResult);
            return Unit.Value;
        }

        var existingNoodleUserId = await _noodleUserRepository.GetByIdAsync(request.Id);
        var existingNoodleUserEmail = await _noodleUserRepository.GetByEmailAsync(request.Email);

        if (existingNoodleUserId != null)
        {
            await NotifyErrorAsync(DomainErrorCodes.UserAlreadyExists, $"User {existingNoodleUserId.Id} already exists");
            return Unit.Value;
        }

        if (existingNoodleUserEmail != null)
        {
            await NotifyErrorAsync(DomainErrorCodes.UserAlreadyExists, $"User {existingNoodleUserEmail.Email} already exists");
            return Unit.Value;
        }

        string passwordHash = BC.HashPassword(request.Password);

        var noodleUser = new NoodleUser(request.Id)
        {
            FullName = request.FullName,
            Email = request.Email,
            Password = passwordHash,
            Status = UserStatus.Pending,
            Role = UserRole.User
        };

        _noodleUserRepository.Add(noodleUser);

        if (await CommitAsync())
        {
            await _bus.Publish(new NoodleUserCreatedEvent(request.Id), cancellationToken);
        }

        return Unit.Value;
    }

    public async Task<Unit> Handle(UpdateNoodleUserCommand request, CancellationToken cancellationToken)
    {
        if (!isAccepted() && IsSelf(request.Id))
        {
            await NotifyErrorAsync(DomainErrorCodes.UserHasNoPermissions, "User must have permissions");
            return Unit.Value;
        }

        var validations = new ValidateUpdateNoodleUser();
        var validationResult = validations.Validate(request);
        if (!validationResult.IsValid)
        {
            await NotifyErrorsAsync(validationResult);
            return Unit.Value;
        }

        var noodleUser = await _noodleUserRepository.GetByIdAsync(request.Id);

        if (noodleUser == null)
        {
            await NotifyErrorAsync(DomainErrorCodes.NotFound, $"User {request.Id} could not be found");
            return Unit.Value;
        }


        if (!IsSelf(request.Id) && !IsAdmin())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserIsNotAuthorized, $"No permission to update user {request.Id}");
            return Unit.Value;
        }

        if (IsAdmin())
        {
            noodleUser.Status = request.Status;
            noodleUser.Role = request.Role;
        }

        noodleUser.FullName = request.FullName;
        noodleUser.Email = request.Email;

        _noodleUserRepository.Update(noodleUser);

        if (await CommitAsync())
        {
            await _bus.Publish(new NoodleUserUpdatedEvent(request.Id), cancellationToken);
        }

        return Unit.Value;
    }

    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var validations = new ValidateChangePassword();
        var validationResult = validations.Validate(request);
        if (!validationResult.IsValid)
        {
            await NotifyErrorsAsync(validationResult);
            return Unit.Value;
        }

        var noodleUser = await _noodleUserRepository.GetByIdAsync(CurrentUserId);

        if (noodleUser == null)
        {
            await NotifyErrorAsync(DomainErrorCodes.NotFound, $"User {CurrentUserId} could not be found");
            return Unit.Value;
        }

        if (!BC.Verify(request.OldPassword, noodleUser.Password))
        {
            await NotifyErrorAsync(DomainErrorCodes.UserOldPasswordNotTheSame, "Old password is incorrect");
            return Unit.Value;
        }

        string passwordHash = BC.HashPassword(request.NewPassword);
        noodleUser.Password = passwordHash;

        _noodleUserRepository.Update(noodleUser);

        if (await CommitAsync())
        {
            await _bus.Publish(new NoodleUserChangedPasswordEvent(CurrentUserId), cancellationToken);
        }

        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteNoodleUserCommand request, CancellationToken cancellationToken)
    {
        if (!isAccepted() && IsSelf(request.Id))
        {
            await NotifyErrorAsync(DomainErrorCodes.UserHasNoPermissions, "User must have permissions");
            return Unit.Value;
        }

        var validations = new ValidateDeleteNoodleUser();
        var validationResult = validations.Validate(request);
        if (!validationResult.IsValid)
        {
            await NotifyErrorsAsync(validationResult);
            return Unit.Value;
        }

        var noodleUser = await _noodleUserRepository.GetByIdAsync(request.Id);

        if (noodleUser == null)
        {
            await NotifyErrorAsync(DomainErrorCodes.NotFound, $"User {request.Id} does not exist");
            return Unit.Value;
        }

        if (!IsSelf(request.Id) && !IsAdmin())
        {
            await NotifyErrorAsync(DomainErrorCodes.UserIsNotAuthorized, $"No permission to delete user {request.Id}");
            return Unit.Value;
        }

        // Delete all items from user in groups too
        var usersInGroups = await _userInGroupRepository.GetByUserAsync(request.Id);

        foreach (var currentUserInGroup in usersInGroups)
        {
            await _userInGroupRepository.RemoveAsync(currentUserInGroup.Id);
        }

        await _noodleUserRepository.RemoveAsync(request.Id);

        if (await CommitAsync())
        {
            foreach (var userInGroup in usersInGroups)
            {
                await _bus.Publish(new UserInGroupDeletedByUserEvent(request.Id, userInGroup.Id), cancellationToken);
            }
            await _bus.Publish(new NoodleUserDeletedEvent(request.Id), cancellationToken);
        }

        return Unit.Value;
    }

    public async Task<string> Handle(LoginNoodleUserCommand request, CancellationToken cancellationToken)
    {
        var validations = new ValidateLoginNoodleUser();
        var validationResult = validations.Validate(request);
        if (!validationResult.IsValid)
        {
            await NotifyErrorsAsync(validationResult);
            return "";
        }

        var noodleUser = await _noodleUserRepository.GetByEmailAsync(request.Email);

        if (noodleUser == null)
        {
            await NotifyErrorAsync(DomainErrorCodes.NotFound, $"User {request.Email} could not be found");
            return "";
        }

        var verifyPassword = BC.Verify(request.Password, noodleUser.Password);
        if (verifyPassword)
        {
            return BuildToken(noodleUser.Email, noodleUser.Status, noodleUser.Role, noodleUser.Id, _tokenSettings);
        }

        await NotifyErrorAsync(DomainErrorCodes.UserPasswordIsIncorrect, "Passwort is incorrect");

        return "";
    }

    public static string BuildToken(string email, UserStatus status, UserRole role, Guid Id, TokenSettings tokenSettings)
    {
        var claims = new[]
        {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.AuthorizationDecision, status.ToString()),
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, Id.ToString())
            };

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(tokenSettings.Secret));

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new JwtSecurityToken(
            tokenSettings.Issuer,
            tokenSettings.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(EXPIRY_DURATION_MINUTES),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
