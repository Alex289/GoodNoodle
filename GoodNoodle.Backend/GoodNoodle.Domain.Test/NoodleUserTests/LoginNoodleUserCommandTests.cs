using GoodNoodle.Domain.CommandHandler;
using GoodNoodle.Domain.Commands;
using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Interfaces;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Domain.Notifications;
using GoodNoodle.Domain.Settings;
using MediatR;
using Moq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Domain.Test.NoodleUserTests;

public class LoginNoodleUserCommandTests
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
    private readonly string _mockUserPassword = "12345asdvseo##";
    private readonly string _mockUserHashedPassword = "$2a$11$lNWxCZtEyLTJ2r9K.cB6jespxrfgCwEcCvFPehE8uy/0kgJgcWFIW";
    private readonly string _mockUserEmail = "email@provider.com";

    public LoginNoodleUserCommandTests()
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

        _tokenSettings.Issuer = "Server";
        _tokenSettings.Audience = "Client";
        _tokenSettings.Secret = "VeryLongTestSuperSecret1234567";

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
            Password = _mockUserHashedPassword
        };

        _noodleUserRepository.Setup(x => x.GetByEmailAsync(_mockUserEmail)).ReturnsAsync(noodleUser);
    }

    [Fact]
    public async Task Should_Login_User()
    {
        // Arrange
        var loginCommand = new LoginNoodleUserCommand(_mockUserEmail, _mockUserPassword);

        // Act
        var token = await _noodleUserCommandHandler.Handle(loginCommand, CancellationToken.None);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);

        var email = jwtSecurityToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
        Assert.Equal(_mockUserEmail, email);

        var Id = jwtSecurityToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
        Assert.Equal(_mockUserId.ToString(), Id);
    }

    [Fact]
    public async Task Should_Not_Login_User()
    {
        // Arrange
        var loginCommand = new LoginNoodleUserCommand("", _mockUserPassword);

        // Act
        var token = await _noodleUserCommandHandler.Handle(loginCommand, CancellationToken.None);

        // Assert
        Assert.Equal("", token);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.UserEmptyEmail), CancellationToken.None));
    }

    [Fact]
    public async Task Should_Not_Login_User_With_Wrong_Password()
    {
        // Arrange
        var loginCommand = new LoginNoodleUserCommand(_mockUserEmail, "Wrong password");

        // Act
        var token = await _noodleUserCommandHandler.Handle(loginCommand, CancellationToken.None);

        // Assert
        Assert.Equal("", token);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.UserPasswordIsIncorrect), CancellationToken.None));
    }
}
