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

public class CreateNoodleUserCommandTests
{
    private readonly Mock<IMediator> _bus;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly NoodleUserCommandHandler _noodleUserCommandHandler;
    private readonly Mock<DomainNotificationHandler> _notificationHandler;
    private readonly Mock<INoodleUserRepository> _noodleUserRepository;
    private readonly Mock<IUserInGroupRepository> _userInGroupRepository;
    private readonly Mock<IUser> _mockUserAccessor;
    private readonly TokenSettings _tokenSettings = new TokenSettings();
    private readonly Guid _mockUserId = Guid.Parse("1993E56A-DA93-4FDA-BF33-AAAE0AD841D2");
    private readonly string _mockUserFullName = "Hans Zimmer";
    private readonly string _mockUserPassword = "12345asDvseo##";
    private readonly string _mockUserEmail = "email@provider.com";

    public CreateNoodleUserCommandTests()
    {
        _bus = new Mock<IMediator>();
        _uow = new Mock<IUnitOfWork>();
        _noodleUserRepository = new Mock<INoodleUserRepository>();
        _userInGroupRepository = new Mock<IUserInGroupRepository>();
        _notificationHandler = new Mock<DomainNotificationHandler>();
        _mockUserAccessor = new Mock<IUser>();

        _mockUserAccessor.Setup(x => x.Id).Returns(_mockUserId);
        _mockUserAccessor.Setup(x => x.Role).Returns(UserRole.Admin.ToString());

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
            Password = _mockUserPassword
        };

        _noodleUserRepository.Setup(x => x.GetByIdAsync(_mockUserId)).ReturnsAsync(noodleUser);
    }

    [Fact]
    public async Task Should_Create_New_User()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userFullName = "Peter Parker";
        var userPassword = "TestPassword123#";
        var userEmail = "email@provider.com";
        var newUserCommand = new CreateNoodleUserCommand(userId, userFullName, userPassword, userEmail);

        // Act
        await _noodleUserCommandHandler.Handle(newUserCommand, CancellationToken.None);

        // Assert
        _noodleUserRepository.Verify(
            x => x.Add(
                It.Is<NoodleUser>(
                    g =>
                        g.Id == userId
                        && g.FullName == userFullName)));

        _uow.Verify(x => x.CommitAsync(), Times.Once);

        _bus.Verify(x => x.Publish(It.Is<NoodleUserCreatedEvent>(g => g.Id == userId), CancellationToken.None));

        Assert.NotEqual(userId, Guid.Empty);
    }

    [Fact]
    public async Task Should_Not_Create_New_User_Invalid_Empty_Id()
    {
        // Arrange
        var userId = Guid.Empty;
        var userFullName = "Peter Parker";
        var userPassword = "TestPassword123#";
        var userEmail = "email@provider.com";
        var newUserCommand = new CreateNoodleUserCommand(userId, userFullName, userPassword, userEmail);

        // Act
        await _noodleUserCommandHandler.Handle(newUserCommand, CancellationToken.None);

        // Assert
        _noodleUserRepository.Verify(
            x => x.Add(
                It.Is<NoodleUser>(
                    g =>
                        g.Id == userId &&
                        g.FullName == userFullName &&
                        g.Password == userPassword)),
            Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<NoodleUserCreatedEvent>(g => g.Id == userId), CancellationToken.None), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.UserEmptyId), CancellationToken.None));
    }

    [Fact]
    public async Task Should_Not_Create_New_User_Already_Exists()
    {
        // Arrange
        var newUserCommand = new CreateNoodleUserCommand(
            _mockUserId,
            _mockUserFullName,
            _mockUserPassword,
            _mockUserEmail);

        // Act
        await _noodleUserCommandHandler.Handle(newUserCommand, CancellationToken.None);

        // Assert
        _noodleUserRepository.Verify(
            x => x.Add(
                It.Is<NoodleUser>(
                    g =>
                        g.Id == _mockUserId &&
                        g.FullName == _mockUserFullName &&
                        g.Password == _mockUserPassword)),
            Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<NoodleUserCreatedEvent>(g => g.Id == _mockUserId), CancellationToken.None), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.UserAlreadyExists), CancellationToken.None));
    }
}
