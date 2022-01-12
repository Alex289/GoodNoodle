using GoodNoodle.Domain.CommandHandler;
using GoodNoodle.Domain.Commands.Stars;
using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Events.Star;
using GoodNoodle.Domain.Interfaces;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Domain.Notifications;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Domain.Test.StarTests;

public class CreateStarCommandTests
{
    private readonly Mock<IMediator> _bus;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly StarCommandHandler _starCommandHandler;
    private readonly Mock<DomainNotificationHandler> _notificationHandler;
    private readonly Mock<IStarRepository> _starRepository;
    private readonly Mock<IUserInGroupRepository> _userInGroupRepository;
    private readonly Mock<IUser> _mockUserAccessor;
    private readonly Guid _mockStarId = Guid.Parse("1993E56A-DA93-4FDA-BF33-AAAE0AD841D2");
    private readonly Guid _mockStarUserId = Guid.Parse("2441C9A1-ED7F-4208-8235-14E00DF9B784");
    private readonly Guid _mockStarGroupId = Guid.Parse("A6CF513C-80B2-4A2A-8787-C42200048C54");
    private readonly string _mockStarReason = "Why not";

    public CreateStarCommandTests()
    {
        _bus = new Mock<IMediator>();
        _uow = new Mock<IUnitOfWork>();
        _starRepository = new Mock<IStarRepository>();
        _notificationHandler = new Mock<DomainNotificationHandler>();
        _userInGroupRepository = new Mock<IUserInGroupRepository>();
        _mockUserAccessor = new Mock<IUser>();

        _mockUserAccessor.Setup(x => x.Id).Returns(Guid.NewGuid());
        _mockUserAccessor.Setup(x => x.Role).Returns(UserRole.Admin.ToString());
        _mockUserAccessor.Setup(x => x.Status).Returns(UserStatus.Accepted.ToString());

        _uow.Setup(x => x.CommitAsync()).ReturnsAsync(true);

        _starCommandHandler = new StarCommandHandler(
            _uow.Object,
            _bus.Object,
            _notificationHandler.Object,
            _starRepository.Object,
            _userInGroupRepository.Object,
            _mockUserAccessor.Object);

        var star = new Star(_mockStarId)
        {
            NoodleGroupId = _mockStarGroupId,
            NoodleUserId = _mockStarUserId,
            Reason = _mockStarReason
        };

        _starRepository.Setup(x => x.GetByIdAsync(_mockStarId)).ReturnsAsync(star);
    }

    [Fact]
    public async Task Should_Create_New_Star()
    {
        // Arrange
        var starId = Guid.NewGuid();
        var starNoodleUserId = Guid.NewGuid();
        var starNoodleGroupId = Guid.NewGuid();
        var starReason = "Why not";
        var newStarCommand = new CreateStarCommand(starId, starNoodleUserId, starNoodleGroupId, starReason);

        // Act
        await _starCommandHandler.Handle(newStarCommand, CancellationToken.None);

        // Assert
        _starRepository.Verify(
            x => x.Add(
                It.Is<Star>(
                    g =>
                        g.Id == starId &&
                        g.NoodleUserId == starNoodleUserId &&
                        g.NoodleGroupId == starNoodleGroupId &&
                        g.Reason == starReason)));

        _uow.Verify(x => x.CommitAsync(), Times.Once);

        _bus.Verify(x => x.Publish(It.Is<CreatedStarEvent>(g => g.Id == starId), CancellationToken.None));
    }

    [Fact]
    public async Task Should_Not_Create_New_Star_Invalid_Empty_Id()
    {
        // Arrange
        var starId = Guid.Empty;
        var starNoodleUserId = Guid.NewGuid();
        var starNoodleGroupId = Guid.NewGuid();
        var starReason = "Why not";
        var newStarCommand = new CreateStarCommand(starId, starNoodleUserId, starNoodleGroupId, starReason);

        // Act
        await _starCommandHandler.Handle(newStarCommand, CancellationToken.None);

        // Assert
        _starRepository.Verify(
            x => x.Add(
                It.Is<Star>(
                    g =>
                        g.Id == starId &&
                        g.NoodleUserId == starNoodleUserId &&
                        g.NoodleGroupId == starNoodleGroupId &&
                        g.Reason == starReason)),
            Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<CreatedStarEvent>(g => g.Id == starId), CancellationToken.None), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.StarEmptyId), CancellationToken.None));
    }

    [Fact]
    public async Task Should_Not_Create_New_Star_Already_Exists()
    {
        // Arrange
        var newStarCommand = new CreateStarCommand(_mockStarId, _mockStarUserId, _mockStarGroupId, _mockStarReason);

        // Act
        await _starCommandHandler.Handle(newStarCommand, CancellationToken.None);

        // Assert
        _starRepository.Verify(
            x => x.Add(
                It.Is<Star>(
                    g =>
                        g.Id == _mockStarId &&
                        g.NoodleUserId == _mockStarUserId &&
                        g.NoodleGroupId == _mockStarGroupId &&
                        g.Reason == _mockStarReason)),
            Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<CreatedStarEvent>(g => g.Id == _mockStarId), CancellationToken.None), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.StarIdAlreadyExists), CancellationToken.None));
    }
}
