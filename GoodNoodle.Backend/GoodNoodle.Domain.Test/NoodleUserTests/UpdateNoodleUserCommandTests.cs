using GoodNoodle.Domain.CommandHandler;
using GoodNoodle.Domain.Commands;
using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Events.NoodleUser;
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

namespace GoodNoodle.Domain.Test.NoodleUserTests;

public class UpdateNoodleUserCommandTests
{
    private readonly Mock<IMediator> _bus;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly NoodleUserCommandHandler _noodleUserCommandHandler;
    private readonly Mock<DomainNotificationHandler> _notificationHandler;
    private readonly Mock<INoodleUserRepository> _noodleUserRepository;
    private readonly Mock<IUserInGroupRepository> _userInGroupRepository;
    private readonly Mock<IUser> _mockUserAccessor;
    private readonly TokenSettings _tokenSettings = new TokenSettings();
    private readonly Guid _mockUserId = Guid.Parse("D57F16CD-DFF3-4580-9F6D-A49007D9CDB7");
    private readonly Guid _mockNonExistingUserId = Guid.Parse("1993E56A-DA93-4FDA-BF33-AAAE0AD841D2");
    private readonly string _mockUserEmail = "email@provider.com";
    private readonly string _mockUserFullName = "Peter Parker";
    private readonly string _mockUserPassword = "TestPassword123#";
    private readonly UserStatus _mockUserStatus = UserStatus.Accepted;

    public UpdateNoodleUserCommandTests()
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
            Email = _mockUserEmail,
            FullName = _mockUserFullName,
            Password = _mockUserPassword,
            Status = _mockUserStatus
        };

        _noodleUserRepository.Setup(x => x.GetByIdAsync(_mockUserId)).ReturnsAsync(noodleUser);
    }

    [Fact]
    public async Task Should_Update_User()
    {
        // Arrange
        var userFullName = "Johnny Depp";
        var status = UserStatus.Declined;
        var email = "johnny@depp.com";
        var role = UserRole.User;
        var updateUserCommand = new UpdateNoodleUserCommand(_mockUserId, userFullName, status, email, role);

        // Act
        await _noodleUserCommandHandler.Handle(updateUserCommand, CancellationToken.None);

        // Assert
        _noodleUserRepository.Verify(
            x => x.Update(
                It.Is<NoodleUser>(
                    g =>
                        g.Id == _mockUserId &&
                        g.FullName == userFullName &&
                        g.Status == status)));

        _uow.Verify(x => x.CommitAsync(), Times.Once);

        _bus.Verify(x => x.Publish(It.Is<NoodleUserUpdatedEvent>(g => g.Id == _mockUserId), CancellationToken.None));
    }

    [Fact]
    public async Task Should_Not_Update_User_Invalid_Empty_Id()
    {
        // Arrange
        var userId = Guid.Empty;
        var userFullName = "Peter Parker";
        var status = UserStatus.Accepted; // come on hes an avenger..
        var email = "peter@parker.com";
        var role = UserRole.User;
        var updateUserCommand = new UpdateNoodleUserCommand(userId, userFullName, status, email, role);

        // Act
        await _noodleUserCommandHandler.Handle(updateUserCommand, CancellationToken.None);

        // Assert
        _noodleUserRepository.Verify(
            x => x.Update(
                It.Is<NoodleUser>(
                    g =>
                        g.Id == userId &&
                        g.FullName == userFullName &&
                        g.Status == status)),
            Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<NoodleUserUpdatedEvent>(g => g.Id == userId), CancellationToken.None), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.UserEmptyId), CancellationToken.None));
    }

    [Fact]
    public async Task Should_Not_Update_User_Not_Exist()
    {
        // Arrange
        var userFullName = "Peter Parker";
        var status = UserStatus.Pending;
        var email = "peter@parker.com";
        var role = UserRole.User;
        var updateUserCommand = new UpdateNoodleUserCommand(_mockNonExistingUserId, userFullName, status, email, role);

        // Act
        await _noodleUserCommandHandler.Handle(updateUserCommand, CancellationToken.None);

        // Assert
        _noodleUserRepository.Verify(
            x => x.Update(
                It.Is<NoodleUser>(
                    g =>
                        g.Id == _mockNonExistingUserId &&
                        g.FullName == userFullName &&
                        g.Status == status)),
            Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<NoodleUserUpdatedEvent>(g => g.Id == _mockNonExistingUserId), CancellationToken.None), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.NotFound), CancellationToken.None));
    }
}
