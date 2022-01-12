using GoodNoodle.Domain.CommandHandler;
using GoodNoodle.Domain.Commands.NoodleUser;
using GoodNoodle.Domain.Entities;
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

public class ChangePasswordCommandTests
{
    private readonly Mock<IMediator> _bus;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly NoodleUserCommandHandler _noodleUserCommandHandler;
    private readonly Mock<DomainNotificationHandler> _notificationHandler;
    private readonly Mock<INoodleUserRepository> _noodleUserRepository;
    private readonly Mock<IUserInGroupRepository> _userInGroupRepository;
    private readonly Mock<IUser> _mockUserAccessor;
    private readonly TokenSettings _tokenSettings;
    private readonly Guid _mockUserId = Guid.Parse("D57F16CD-DFF3-4580-9F6D-A49007D9CDB7");
    private readonly Guid _mockNonExistingUserId = Guid.Parse("1993E56A-DA93-4FDA-BF33-AAAE0AD841D2");
    private readonly string _mockUserFullName = "Peter Parker";
    private readonly string _mockUserPassword = "12345asdvseo##";
    private readonly string _mockUserHashedPassword = "$2a$11$lNWxCZtEyLTJ2r9K.cB6jespxrfgCwEcCvFPehE8uy/0kgJgcWFIW";
    private readonly UserStatus _mockUserStatus = UserStatus.Accepted;

    public ChangePasswordCommandTests()
    {
        _bus = new Mock<IMediator>();
        _uow = new Mock<IUnitOfWork>();
        _noodleUserRepository = new Mock<INoodleUserRepository>();
        _userInGroupRepository = new Mock<IUserInGroupRepository>();
        _notificationHandler = new Mock<DomainNotificationHandler>();
        _tokenSettings = new TokenSettings();

        _tokenSettings.Issuer = "Server";
        _tokenSettings.Audience = "Client";
        _tokenSettings.Secret = "VeryLongTestSuperSecret1234567";

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
            Password = _mockUserHashedPassword,
            Status = _mockUserStatus
        };

        _noodleUserRepository.Setup(x => x.GetByIdAsync(_mockUserId)).ReturnsAsync(noodleUser);
    }

    [Fact]
    public async Task Should_Update_Password()
    {
        // Arrange
        var userNewPassword = "Johnny123#";
        var updateUserCommand = new ChangePasswordCommand(_mockUserPassword, userNewPassword);

        // Act
        await _noodleUserCommandHandler.Handle(updateUserCommand, CancellationToken.None);

        // Assert
        _noodleUserRepository.Verify(
            x => x.Update(
                It.Is<NoodleUser>(
                    g =>
                        g.Id == _mockUserId)));

        _uow.Verify(x => x.CommitAsync(), Times.Once);

        _bus.Verify(x => x.Publish(It.Is<NoodleUserChangedPasswordEvent>(g => g.Id == _mockUserId), CancellationToken.None));
    }
}
