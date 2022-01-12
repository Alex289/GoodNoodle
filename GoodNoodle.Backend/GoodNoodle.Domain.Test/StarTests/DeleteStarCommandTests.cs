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

public class DeleteStarCommandTests
{
    private readonly Mock<IMediator> _bus;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly StarCommandHandler _starCommandHandler;
    private readonly Mock<DomainNotificationHandler> _notificationHandler;
    private readonly Mock<IStarRepository> _starRepository;
    private readonly Mock<IUserInGroupRepository> _userInGroupRepository;
    private readonly Mock<IUser> _mockUserAccessor;
    private readonly Guid _mockStarId = Guid.Parse("35D362D3-8CCD-4B07-83CF-2B87F0B0DEBB");
    private readonly Guid _mockStarNoodleUserId = Guid.Parse("10F7AAB2-ABE7-4419-A7F9-12FA7EF51AAB");
    private readonly Guid _mockStarNoodleGroupId = Guid.Parse("0FC70164-C256-4508-9E3E-6B24A03D6FD3");
    private readonly Guid _mockNonExistingStarId = Guid.Parse("1993E56A-DA93-4FDA-BF33-AAAE0AD841D2");
    private readonly string _mockStarReason = "Why not";

    public DeleteStarCommandTests()
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

        var _mockStar = new Star(_mockStarId)
        {
            NoodleUserId = _mockStarNoodleUserId,
            NoodleGroupId = _mockStarNoodleGroupId,
            Reason = _mockStarReason
        };

        _starRepository.Setup(x => x.GetByIdAsync(_mockStarId)).ReturnsAsync(_mockStar);
    }

    [Fact]
    public async Task Should_Delete_Star()
    {
        // Arrange
        var deleteStarCommand = new DeleteStarCommand(_mockStarId);

        // Act
        await _starCommandHandler.Handle(deleteStarCommand, CancellationToken.None);

        // Assert
        _starRepository.Verify(x => x.RemoveAsync(It.Is<Guid>(g => g == _mockStarId)));

        _uow.Verify(x => x.CommitAsync(), Times.Once);

        _bus.Verify(x => x.Publish(It.Is<DeletedStarEvent>(g => g.Id == _mockStarId), CancellationToken.None));
    }

    [Fact]
    public async Task Should_Not_Delete_Star_Invalid_Empty_Id()
    {
        // Arrange
        var deleteStarCommand = new DeleteStarCommand(Guid.Empty);

        // Act
        await _starCommandHandler.Handle(deleteStarCommand, CancellationToken.None);

        // Assert
        _starRepository.Verify(x => x.RemoveAsync(It.Is<Guid>(g => g == _mockStarId)), Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DeletedStarEvent>(g => g.Id == _mockStarId), CancellationToken.None), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.StarEmptyId), CancellationToken.None));
    }

    [Fact]
    public async Task Should_Not_Delete_Star_Not_Exist()
    {
        // Arrange
        var deleteStarCommand = new DeleteStarCommand(_mockNonExistingStarId);

        // Act
        await _starCommandHandler.Handle(deleteStarCommand, CancellationToken.None);

        // Assert
        _starRepository.Verify(x => x.RemoveAsync(It.Is<Guid>(g => g == _mockNonExistingStarId)), Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DeletedStarEvent>(g => g.Id == _mockStarId), CancellationToken.None), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.NotFound), CancellationToken.None));
    }
}
