using GoodNoodle.Application.ViewModel.UserInGroup;
using System;

namespace GoodNoodle.IntegrationTests.TestFixtures;

public class TestUserGroupFixture
{
    public UserInGroupViewModel UserInGroupViewModel { get; set; }
    public string BearerToken { get; set; }
    public Guid InvitedGroupId { get; set; }
    public Guid InvitationId { get; set; }
}
