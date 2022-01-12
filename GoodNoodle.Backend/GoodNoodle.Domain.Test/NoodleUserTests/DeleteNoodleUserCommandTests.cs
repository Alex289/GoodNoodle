using GoodNoodle.Domain.CommandHandler;
using GoodNoodle.Domain.Commands;
using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Events.NoodleUser;
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

namespace GoodNoodle.Domain.Test.NoodleUserTests;

public class DeleteNoodleUserCommandTests
{
    private readonly Mock<IMediator> _bus;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly NoodleUserCommandHandler _noodleUserCommandHandler;
    private readonly Mock<DomainNotificationHandler> _notificationHandler;
    private readonly Mock<INoodleUserRepository> _noodleUserRepository;
    private readonly Mock<IUserInGroupRepository> _userInGroupRepository;
    private readonly TokenSettings _tokenSettings = new TokenSettings();
    private readonly Mock<IUser> _mockUserAccessor;
    private readonly Guid _mockUserId = Guid.Parse("D57F16CD-DFF3-4580-9F6D-A49007D9CDB7");
    private readonly Guid _mockUserInGroupId = Guid.Parse("FBA14E1F-8A73-4875-A7C6-CE4484424382");
    private readonly Guid _mockNonExistingUserId = Guid.Parse("1993E56A-DA93-4FDA-BF33-AAAE0AD841D2");
    private readonly string _mockUserFullName = "Peter Parker";
    private readonly string _mockUserPassword = "Strong Password";
    private readonly UserStatus _mockUserStatus = UserStatus.Accepted;

    public DeleteNoodleUserCommandTests()
    {
        _bus = new Mock<IMediator>();
        _uow = new Mock<IUnitOfWork>();
        _noodleUserRepository = new Mock<INoodleUserRepository>();
        _userInGroupRepository = new Mock<IUserInGroupRepository>();
        _notificationHandler = new Mock<DomainNotificationHandler>();
        _mockUserAccessor = new Mock<IUser>();

        _mockUserAccessor.Setup(x => x.Id).Returns(_mockUserId);
        _mockUserAccessor.Setup(x => x.Role).Returns(UserRole.Admin.ToString());
        _mockUserAccessor.Setup(x => x.Status).Returns(UserStatus.Accepted.ToString());

        _uow.Setup(x => x.CommitAsync()).ReturnsAsync(true);

        _noodleUserCommandHandler = new NoodleUserCommandHandler(
            _uow.Object,
            _bus.Object,
            _notificationHandler.Object,
            _noodleUserRepository.Object,
            _userInGroupRepository.Object,
            _tokenSettings,
            _mockUserAccessor.Object);

        var noodleUser = new NoodleUser(_mockUserId)
        {
            FullName = _mockUserFullName,
            Password = _mockUserPassword,
            Status = _mockUserStatus
        };

        _noodleUserRepository.Setup(x => x.GetByIdAsync(_mockUserId)).ReturnsAsync(noodleUser);

        var userInGroupList = new List<UserInGroup>();

        var userInGroup = new UserInGroup(_mockUserInGroupId)
        {
            NoodleGroupId = Guid.NewGuid(),
            NoodleUserId = _mockUserId,
            Role = GroupRole.Student
        };
        userInGroupList.Add(userInGroup);

        _userInGroupRepository.Setup(a => a.GetByUserAsync(_mockUserId)).ReturnsAsync(userInGroupList);
    }

    [Fact]
    public async Task Should_Delete_User()
    {
        // Arrange
        var deleteUserCommand = new DeleteNoodleUserCommand(_mockUserId);

        // Act
        await _noodleUserCommandHandler.Handle(deleteUserCommand, CancellationToken.None);

        // Assert
        _noodleUserRepository.Verify(x => x.RemoveAsync(It.Is<Guid>(g => g == _mockUserId)));

        _uow.Verify(x => x.CommitAsync(), Times.Once);

        _bus.Verify(x => x.Publish(It.Is<NoodleUserDeletedEvent>(g => g.Id == _mockUserId), CancellationToken.None));

        _userInGroupRepository.Verify(x => x.RemoveAsync(It.Is<Guid>(g => g == _mockUserInGroupId)));

        _bus.Verify(
            x => x.Publish(
                It.Is<UserInGroupDeletedByUserEvent>(
                    g =>
                        g.NoodleUserId == _mockUserId &&
                        g.Id == _mockUserInGroupId),
                CancellationToken.None));

        _bus.Verify(x => x.Publish(It.Is<UserInGroupDeletedByUserEvent>(g => g.NoodleUserId == _mockUserId), CancellationToken.None), Times.Once);

    }

    [Fact]
    public async Task Should_Not_Delete_User_Invalid_Empty_Id()
    {
        // Arrange
        var userId = Guid.Empty;
        var deleteUserCommand = new DeleteNoodleUserCommand(userId);

        // Act
        await _noodleUserCommandHandler.Handle(deleteUserCommand, CancellationToken.None);

        // Assert
        _noodleUserRepository.Verify(x => x.RemoveAsync(It.Is<Guid>(g => g == _mockUserId)), Times.Never);

        _userInGroupRepository.Verify(x => x.RemoveAsync(It.Is<Guid>(g => g == _mockUserInGroupId)), Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<NoodleUserDeletedEvent>(g => g.Id == userId), CancellationToken.None), Times.Never);

        _bus.Verify(
            x => x.Publish(
                It.Is<UserInGroupDeletedByUserEvent>(
                    g =>
                        g.NoodleUserId == _mockUserId),
                CancellationToken.None),
            Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.UserEmptyId), CancellationToken.None));
    }

    [Fact]
    public async Task Should_Not_Delete_User_Not_Exist()
    {
        // Arrange
        var deleteUserCommand = new DeleteNoodleUserCommand(_mockNonExistingUserId);

        // Act
        await _noodleUserCommandHandler.Handle(deleteUserCommand, CancellationToken.None);

        // Assert
        _noodleUserRepository.Verify(x => x.RemoveAsync(It.Is<Guid>(g => g == _mockUserId)), Times.Never);

        _userInGroupRepository.Verify(x => x.RemoveAsync(It.Is<Guid>(g => g == _mockUserInGroupId)), Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<NoodleUserDeletedEvent>(g => g.Id == _mockNonExistingUserId), CancellationToken.None), Times.Never);

        _bus.Verify(
            x => x.Publish(
                It.Is<UserInGroupDeletedByUserEvent>(
                    g =>
                        g.NoodleUserId == _mockUserId),
                CancellationToken.None),
            Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.NotFound), CancellationToken.None));
    }
}
