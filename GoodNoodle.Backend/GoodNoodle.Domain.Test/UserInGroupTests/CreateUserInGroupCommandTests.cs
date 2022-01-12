using GoodNoodle.Domain.CommandHandler;
using GoodNoodle.Domain.Commands.UserInGroup;
using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Events.Invitations;
using GoodNoodle.Domain.Events.UserInGroup;
using GoodNoodle.Domain.Interfaces;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Domain.Notifications;
using GoodNoodle.Domain.Settings;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Domain.Test.UserInGroupTests;

public class CreateUserInGroupCommandTests
{
    private readonly Mock<IMediator> _bus;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly UserInGroupCommandHandler _userInGroupCommandHandler;
    private readonly Mock<DomainNotificationHandler> _notificationHandler;
    private readonly Mock<IUserInGroupRepository> _userInGroupRepository;
    private readonly Mock<IInvitationsRepository> _invitationsRepositoy;
    private readonly Mock<IStarRepository> _starRepository;
    private readonly Mock<IUser> _mockUserAccessor;
    private readonly Guid _mockUserInGroupId = Guid.Parse("1993E56A-DA93-4FDA-BF33-AAAE0AD841D2");
    private readonly Guid _mockUserId = Guid.Parse("2441C9A1-ED7F-4208-8235-14E00DF9B784");
    private readonly Guid _mockGroupId = Guid.Parse("A6CF513C-80B2-4A2A-8787-C42200048C54");
    private readonly Guid _mockInvitationId = Guid.Parse("080BEFDB-5833-4DE9-B6E1-69497C5BD7A4");
    private readonly Guid _mockNewInvitationId = Guid.Parse("0763C440-26F3-4E3D-810C-B6D3C6595B36");
    private readonly GroupRole _mockRole = GroupRole.Student;

    public CreateUserInGroupCommandTests()
    {
        _bus = new Mock<IMediator>();
        _uow = new Mock<IUnitOfWork>();
        _userInGroupRepository = new Mock<IUserInGroupRepository>();
        _starRepository = new Mock<IStarRepository>();
        _invitationsRepositoy = new Mock<IInvitationsRepository>();
        _notificationHandler = new Mock<DomainNotificationHandler>();
        _mockUserAccessor = new Mock<IUser>();

        _mockUserAccessor.Setup(x => x.Id).Returns(_mockUserId);
        _mockUserAccessor.Setup(x => x.Role).Returns(UserRole.Admin.ToString());
        _mockUserAccessor.Setup(x => x.Status).Returns(UserStatus.Accepted.ToString());

        _uow.Setup(x => x.CommitAsync()).ReturnsAsync(true);

        _userInGroupCommandHandler = new UserInGroupCommandHandler(
            _uow.Object,
            _bus.Object,
            _notificationHandler.Object,
            _userInGroupRepository.Object,
            _invitationsRepositoy.Object,
            _starRepository.Object,
            _mockUserAccessor.Object,
            new MailSettings(),
            new HostingSettings());

        var userInGroup = new UserInGroup(_mockUserInGroupId)
        {
            NoodleGroupId = _mockGroupId,
            NoodleUserId = _mockUserId,
            Role = _mockRole
        };

        var userInGroupList = new List<UserInGroup>();
        userInGroupList.Add(userInGroup);

        _userInGroupRepository.Setup(x => x.GetByIdAsync(_mockUserInGroupId)).ReturnsAsync(userInGroup);
        _userInGroupRepository.Setup(x => x.GetByUserAsync(_mockUserId)).ReturnsAsync(userInGroupList);

        var invitation = new Invitations(_mockInvitationId)
        {
            NoodleGroupId = _mockGroupId,
            NoodleUserId = _mockUserId,
            Role = _mockRole
        };

        var newInvitation = new Invitations(_mockNewInvitationId)
        {
            NoodleGroupId = Guid.NewGuid(),
            NoodleUserId = _mockUserId,
            Role = _mockRole
        };

        _invitationsRepositoy.Setup(x => x.GetByIdAsync(_mockInvitationId)).ReturnsAsync(invitation);
        _invitationsRepositoy.Setup(x => x.GetByIdAsync(_mockNewInvitationId)).ReturnsAsync(newInvitation);
    }

    [Fact]
    public async Task Should_Create_New_UserInGroup()
    {
        // Arrange
        var userInGroupId = Guid.NewGuid();
        var newUserInGroupCommand = new CreateUserInGroupCommand(_mockNewInvitationId, userInGroupId);

        // Act
        await _userInGroupCommandHandler.Handle(newUserInGroupCommand, CancellationToken.None);

        // Assert
        _userInGroupRepository.Verify(
            x => x.Add(
                It.Is<UserInGroup>(
                    g =>
                        g.Id == userInGroupId)));

        _uow.Verify(x => x.CommitAsync(), Times.Once);

        _bus.Verify(x => x.Publish(It.Is<UserInGroupCreatedEvent>(g => g.Id == userInGroupId), CancellationToken.None));
        _bus.Verify(x => x.Publish(It.Is<DeletedInvitationEvent>(g => g.Id == _mockNewInvitationId), CancellationToken.None));
    }

    [Fact]
    public async Task Should_Not_Create_New_UserInGroup_Invalid_Empty_Id()
    {
        // Arrange
        var invitationId = Guid.NewGuid();
        var userInGroupId = Guid.Empty;
        var newUserInGroupCommand = new CreateUserInGroupCommand(invitationId, userInGroupId);

        // Act
        await _userInGroupCommandHandler.Handle(newUserInGroupCommand, CancellationToken.None);

        // Assert
        _userInGroupRepository.Verify(
            x => x.Add(
                It.Is<UserInGroup>(
                    g =>
                        g.Id == userInGroupId)),
            Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<UserInGroupCreatedEvent>(g => g.Id == userInGroupId), CancellationToken.None), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.UserInGroupIdEmpty), CancellationToken.None));
    }

    [Fact]
    public async Task Should_Not_Create_New_UserInGroup_Already_Exists()
    {
        // Arrange
        var newUserInGroupCommand = new CreateUserInGroupCommand(_mockInvitationId, _mockUserInGroupId);

        // Act
        await _userInGroupCommandHandler.Handle(newUserInGroupCommand, CancellationToken.None);

        // Assert
        _userInGroupRepository.Verify(
            x => x.Add(
                It.Is<UserInGroup>(
                    g =>
                        g.Id == _mockUserInGroupId &&
                        g.NoodleUserId == _mockUserId &&
                        g.NoodleGroupId == _mockGroupId)),
            Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<UserInGroupCreatedEvent>(g => g.Id == _mockUserInGroupId), CancellationToken.None), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.UserInGroupAlreadyExists), CancellationToken.None));
    }
}
