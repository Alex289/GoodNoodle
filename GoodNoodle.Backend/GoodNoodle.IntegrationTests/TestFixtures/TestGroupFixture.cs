using GoodNoodle.Application.ViewModel.NoodleGroup;
using System;

namespace GoodNoodle.IntegrationTests.TestFixtures;

public class TestGroupFixture
{
    public NoodleGroupViewModel GroupViewModel { get; set; }
    public Guid UserInGroupId { get; set; }
    public string BearerToken { get; set; }
    public Guid NoodleUserId { get; set; }
}
