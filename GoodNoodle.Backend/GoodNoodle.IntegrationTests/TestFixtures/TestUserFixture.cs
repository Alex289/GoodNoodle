using GoodNoodle.Application.ViewModel.NoodleUser;
using System;

namespace GoodNoodle.IntegrationTests.TestFixtures;

public class TestUserFixture
{
    public NoodleUserViewModel UserViewModel { get; set; }

    public Guid UserInGroupId { get; set; }
    public string NewCreatedUserToken { get; set; }
    public Guid GroupId { get; set; }
    public string BearerToken { get; set; }
}
