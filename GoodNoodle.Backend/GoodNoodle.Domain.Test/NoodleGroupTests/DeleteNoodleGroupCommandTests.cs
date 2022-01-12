using GoodNoodle.Domain.CommandHandler;
using GoodNoodle.Domain.Commands.NoodleGroup;
using GoodNoodle.Domain.Entities;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Events.NoodleGroup;
using GoodNoodle.Domain.Events.UserInGroup;
using GoodNoodle.Domain.Interfaces;
using GoodNoodle.Domain.Interfaces.Repositories;
using GoodNoodle.Domain.Notifications;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Domain.Test.NoodleGroupTests;

public class DeleteNoodleGroupCommandTests
{
    private readonly Mock<IMediator> _bus;
    private readonly Mock<IUnitOfWork> _uow;
    private readonly NoodleGroupCommandHandler _noodleGroupCommandHandler;
    private readonly Mock<DomainNotificationHandler> _notificationHandler;
    private readonly Mock<INoodleGroupRepository> _noodleGroupRepository;
    private readonly Mock<IUserInGroupRepository> _userInGroupRepository;
    private readonly Mock<IUser> _mockUserAccessor;
    private readonly Guid _mockGroupId = Guid.Parse("A22EB83F-EE08-40FC-9FA9-52BF73792A84");
    private readonly Guid _mockNonExistingGroupId = Guid.Parse("1993E56A-DA93-4FDA-BF33-AAAE0AD841D2");
    private readonly Guid _mockUserInGroupId = Guid.Parse("FBA14E1F-8A73-4875-A7C6-CE4484424382");

    public DeleteNoodleGroupCommandTests()
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

        var group = new NoodleGroup(_mockGroupId) { Name = "Test group", Image = "Test image" };
        _noodleGroupRepository.Setup(a => a.GetByIdAsync(_mockGroupId)).ReturnsAsync(group);

        _noodleGroupRepository.Setup(a => a.GetByIdAsync(_mockNonExistingGroupId)).ReturnsAsync((NoodleGroup)null);

        var userInGroupList = new List<UserInGroup>();

        var userInGroup = new UserInGroup(_mockUserInGroupId)
        {
            NoodleGroupId = _mockGroupId,
            NoodleUserId = Guid.NewGuid(),
            Role = GroupRole.Student
        };
        userInGroupList.Add(userInGroup);

        _userInGroupRepository.Setup(a => a.GetByGroupAsync(_mockGroupId)).ReturnsAsync(userInGroupList);
    }

    [Fact]
    public async Task Should_Delete_Group()
    {
        // Arrange
        var deleteGroupCommand = new DeleteNoodleGroupCommand(_mockGroupId);

        // Act
        await _noodleGroupCommandHandler.Handle(deleteGroupCommand, CancellationToken.None);

        // Assert
        _noodleGroupRepository.Verify(x => x.RemoveAsync(It.Is<Guid>(g => g == _mockGroupId)));

        _userInGroupRepository.Verify(x => x.RemoveAsync(It.Is<Guid>(g => g == _mockUserInGroupId)));

        _uow.Verify(x => x.CommitAsync(), Times.Once);

        _bus.Verify(x => x.Publish(It.Is<NoodleGroupDeletedEvent>(g => g.NoodleGroupId == _mockGroupId), CancellationToken.None));

        _bus.Verify(
            x => x.Publish(
                It.Is<UserInGroupDeletedByGroupEvent>(
                    g =>
                        g.NoodleGroupId == _mockGroupId &&
                        g.Id == _mockUserInGroupId),
                CancellationToken.None));

        _bus.Verify(x => x.Publish(It.Is<UserInGroupDeletedByGroupEvent>(g => g.NoodleGroupId == _mockGroupId), CancellationToken.None), Times.Once);

        Assert.NotEqual(_mockGroupId, Guid.Empty);
    }

    [Fact]
    public async Task Should_Not_Delete_Group_Invalid_Empty_Id()
    {
        // Arrange
        var deleteGroupCommand = new DeleteNoodleGroupCommand(Guid.Empty);

        // Act
        await _noodleGroupCommandHandler.Handle(deleteGroupCommand, CancellationToken.None);

        // Assert
        _noodleGroupRepository.Verify(x => x.RemoveAsync(It.Is<Guid>(g => g == _mockGroupId)), Times.Never);

        _userInGroupRepository.Verify(x => x.RemoveAsync(It.Is<Guid>(g => g == _mockUserInGroupId)), Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(
            It.Is<NoodleGroupDeletedEvent>(
                g =>
                    g.NoodleGroupId == _mockGroupId),
            CancellationToken.None),
            Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.GroupEmptyId), CancellationToken.None));
    }

    [Fact]
    public async Task Should_Not_Delete_Group_Not_Exist()
    {
        // Arrange
        var deleteGroupCommand = new DeleteNoodleGroupCommand(_mockNonExistingGroupId);

        // Act
        await _noodleGroupCommandHandler.Handle(deleteGroupCommand, CancellationToken.None);

        // Assert
        _noodleGroupRepository.Verify(x => x.RemoveAsync(It.Is<Guid>(g => g == _mockNonExistingGroupId)), Times.Never);

        _userInGroupRepository.Verify(x => x.RemoveAsync(It.Is<Guid>(g => g == _mockUserInGroupId)), Times.Never);

        _uow.Verify(x => x.CommitAsync(), Times.Never);

        _bus.Verify(x => x.Publish(
            It.Is<NoodleGroupDeletedEvent>(
                g =>
                    g.NoodleGroupId == _mockGroupId),
            CancellationToken.None),
            Times.Never);

        _bus.Verify(x => x.Publish(It.Is<DomainNotification>(n => n.Error.Key == DomainErrorCodes.NotFound), CancellationToken.None));
    }
}
