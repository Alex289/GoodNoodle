using GoodNoodle.Domain.CommandHandler;
using GoodNoodle.Domain.Commands.NoodleGroup;
using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Events.NoodleGroup;
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

namespace GoodNoodle.Domain.Test.NoodleGroupTests;

public class CreateNoodleGroupCommandTests
{
    private readonly Mock<IMediator> _bus;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly NoodleGroupCommandHandler _noodleGroupCommandHandler;
    private readonly Mock<DomainNotificationHandler> _notificationHandler;
    private readonly Mock<INoodleGroupRepository> _noodleGroupRepository;
    private readonly Mock<IUserInGroupRepository> _userInGroupRepository;
    private readonly Mock<IUser> _mockUserAccessor;
    private readonly Guid _mockGroupId = Guid.Parse("1993E56A-DA93-4FDA-BF33-AAAE0AD841D2");
    private readonly string _mockGroupName = "Coole Gruppe";
    private readonly string _mockGroupImage = "7PuAiwHl9OzqyuB0AXTSSO+fBVg3iZGddNT8BCLeOzY=";
    public CreateNoodleGroupCommandTests()
    {
        _bus = new Mock<IMediator>();
        _uow = new Mock<IUnitOfWork>();
        _noodleGroupRepository = new Mock<INoodleGroupRepository>();
        _userInGroupRepository = new Mock<IUserInGroupRepository>();
        _notificationHandler = new Mock<DomainNotificationHandler>();
        _mockUserAccessor = new Mock<IUser>();

        _mockUserAccessor.Setup(x => x.Id).Returns(Guid.NewGuid());
        _mockUserAccessor.Setup(x => x.Role).Returns(UserRole.Admin.ToString());
        _mockUserAccessor.Setup(x => x.Status).Returns(UserStatus.Accepted.ToString());

        _uow.Setup(x => x.CommitAsync()).ReturnsAsync(true);

        _noodleGroupCommandHandler = new NoodleGroupCommandHandler(
            _uow.Object,
            _bus.Object,
            _noodleGroupRepository.Object,
            _userInGroupRepository.Object,
            _notificationHandler.Object,
            _mockUserAccessor.Object);

        var noodleGroup = new NoodleGroup(_mockGroupId)
        {
            Name = _mockGroupName,
            Image = _mockGroupImage
        };

        _noodleGroupRepository.Setup(x => x.GetByIdAsync(_mockGroupId)).ReturnsAsync(noodleGroup);
    }

    [Fact]
    public async Task Should_Create_New_Group()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var groupName = "Coole Gruppe";
        var groupImage = "7PuAiwHl9OzqyuB0AXTSSO+fBVg3iZGddNT8BCLeOzY=";
        var newGroupCommand = new CreateNoodleGroupCommand(groupId, groupName, groupImage);

        // Act
        await _noodleGroupCommandHandler.Handle(newGroupCommand, CancellationToken.None);

        // Assert
        _noodleGroupRepository.Verify(
            x => x.Add(
                It.Is<NoodleGroup>(
                    g =>
                        g.Id == groupId &&
                        g.Name == groupName
                        && g.Image == groupImage)));

        _uow.Verify(x => x.CommitAsync(), Times.Once);

        _bus.Verify(x => x.Publish(It.Is<NoodleGroupCreatedEvent>(g => g.NoodleGroupId == groupId), CancellationToken.None));

        Assert.NotEqual(groupId, Guid.Empty);
    }

    [Fact]
    public async Task Should_Not_Create_New_Group_Invalid_Empty_Id()
    {
        // Arrange
        var groupId = Guid.Empty;
        var groupName = "Coole Gruppe";
        var groupImage = "7PuAiwHl9OzqyuB0AXTSSO+fBVg3iZGddNT8BCLeOzY=";
        var newGroupCommand = new CreateNoodleGroupCommand(groupId, groupName, groupImage);

        // Act
        await _noodleGroupCommandHandler.Handle(newGroupCommand, CancellationToken.None);

        // Assert
        _noodleGroupRepository.Verify(
            x => x.Add(
                It.Is<NoodleGroup>(
                    g =>
                        g.Id == groupId &&
                        g.Name == groupName &&
                        g.Image == groupImage)),
            Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<NoodleGroupCreatedEvent>(g => g.NoodleGroupId == groupId), CancellationToken.None), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.GroupEmptyId), CancellationToken.None));
    }

    [Fact]
    public async Task Should_Not_Create_New_Group_Already_Exists()
    {
        // Arrange
        var newGroupCommand = new CreateNoodleGroupCommand(_mockGroupId, _mockGroupName, _mockGroupImage);

        // Act
        await _noodleGroupCommandHandler.Handle(newGroupCommand, CancellationToken.None);

        // Assert
        _noodleGroupRepository.Verify(
            x => x.Add(
                It.Is<NoodleGroup>(
                    g =>
                        g.Id == _mockGroupId &&
                        g.Name == _mockGroupName &&
                        g.Image == _mockGroupImage)),
            Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<NoodleGroupCreatedEvent>(g => g.NoodleGroupId == _mockGroupId), CancellationToken.None), Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.GroupAlreadyExist), CancellationToken.None));
    }
}
