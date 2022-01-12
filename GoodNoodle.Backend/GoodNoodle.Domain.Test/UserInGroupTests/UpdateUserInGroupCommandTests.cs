using GoodNoodle.Domain.CommandHandler;
using GoodNoodle.Domain.Commands.UserInGroup;
using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Events.UserInGroup;
using GoodNoodle.Domain.Interfaces;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Domain.Notifications;
using GoodNoodle.Domain.Settings;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Domain.Test.UserInGroupTests;

public class UpdateUserInGroupCommandTests
{
    private readonly Mock<IMediator> _bus;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly UserInGroupCommandHandler _userInGroupCommandHandler;
    private readonly Mock<DomainNotificationHandler> _notificationHandler;
    private readonly Mock<IUserInGroupRepository> _userInGroupRepository;
    private readonly Mock<IInvitationsRepository> _invitationsRepository;
    private readonly Mock<IStarRepository> _starRepository;
    private readonly Mock<IUser> _mockUserAccessor;
    private readonly Guid _mockUserInGroupId = Guid.Parse("35D362D3-8CCD-4B07-83CF-2B87F0B0DEBB");
    private readonly Guid _mockStarNoodleUserId = Guid.Parse("10F7AAB2-ABE7-4419-A7F9-12FA7EF51AAB");
    private readonly Guid _mockStarNoodleGroupId = Guid.Parse("0FC70164-C256-4508-9E3E-6B24A03D6FD3");
    private readonly Guid _mockNonExistingUserInGroupId = Guid.Parse("1993E56A-DA93-4FDA-BF33-AAAE0AD841D2");

    public UpdateUserInGroupCommandTests()
    {
        _bus = new Mock<IMediator>();
        _uow = new Mock<IUnitOfWork>();
        _userInGroupRepository = new Mock<IUserInGroupRepository>();
        _invitationsRepository = new Mock<IInvitationsRepository>();
        _starRepository = new Mock<IStarRepository>();
        _notificationHandler = new Mock<DomainNotificationHandler>();
        _mockUserAccessor = new Mock<IUser>();

        _mockUserAccessor.Setup(x => x.Id).Returns(_mockStarNoodleUserId);
        _mockUserAccessor.Setup(x => x.Role).Returns(UserRole.Admin.ToString());
        _mockUserAccessor.Setup(x => x.Status).Returns(UserStatus.Accepted.ToString());

        _uow.Setup(x => x.CommitAsync()).ReturnsAsync(true);

        _userInGroupCommandHandler = new UserInGroupCommandHandler(
            _uow.Object,
            _bus.Object,
            _notificationHandler.Object,
            _userInGroupRepository.Object,
            _invitationsRepository.Object,
            _starRepository.Object,
            _mockUserAccessor.Object,
            new MailSettings(),
            new HostingSettings());

        var _mockUserInGroup = new UserInGroup(_mockUserInGroupId)
        {
            NoodleUserId = _mockStarNoodleUserId,
            NoodleGroupId = _mockStarNoodleGroupId
        };

        _userInGroupRepository.Setup(x => x.GetByIdAsync(_mockUserInGroupId)).ReturnsAsync(_mockUserInGroup);
    }

    [Fact]
    public async Task Should_Update_UserInGroup()
    {
        // Arrange
        var newUserRole = GroupRole.Teacher;
        var updateUserInGroupCommand = new UpdateUserInGroupCommand(_mockUserInGroupId, Guid.NewGuid(), Guid.NewGuid(), newUserRole);

        // Act
        await _userInGroupCommandHandler.Handle(updateUserInGroupCommand, CancellationToken.None);

        // Assert
        _userInGroupRepository.Verify(
            x => x.Update(
                It.Is<UserInGroup>(
                    g =>
                        g.Id == _mockUserInGroupId)));

        _uow.Verify(x => x.CommitAsync(), Times.Once);

        _bus.Verify(x => x.Publish(It.Is<UserInGroupUpdatedEvent>(g => g.Id == _mockUserInGroupId), CancellationToken.None));
    }

    [Fact]
    public async Task Should_Not_Update_UserInGroup_Invalid_Empty_Id()
    {
        // Arrange
        var newUserRole = GroupRole.Teacher;
        var updateUserInGroupCommand = new UpdateUserInGroupCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), newUserRole);

        // Act
        await _userInGroupCommandHandler.Handle(updateUserInGroupCommand, CancellationToken.None);

        // Assert
        _userInGroupRepository.Verify(
            x => x.Update(
                It.Is<UserInGroup>(
                    g =>
                        g.Id == _mockUserInGroupId)),
            Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<UserInGroupUpdatedEvent>(g => g.Id == _mockUserInGroupId), CancellationToken.None), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.UserInGroupIdEmpty), CancellationToken.None));
    }

    [Fact]
    public async Task Should_Not_Update_UserInGroup_Not_Exist()
    {
        // Arrange
        var newUserRole = GroupRole.Teacher;
        var updateUserInGroupCommand = new UpdateUserInGroupCommand(_mockNonExistingUserInGroupId, Guid.NewGuid(), Guid.NewGuid(), newUserRole);

        // Act
        await _userInGroupCommandHandler.Handle(updateUserInGroupCommand, CancellationToken.None);

        // Assert
        _userInGroupRepository.Verify(
            x => x.Update(
                It.Is<UserInGroup>(
                    g =>
                        g.Id == _mockNonExistingUserInGroupId)),
            Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<UserInGroupUpdatedEvent>(g => g.Id == _mockNonExistingUserInGroupId), CancellationToken.None), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.NotFound), CancellationToken.None));
    }
}
