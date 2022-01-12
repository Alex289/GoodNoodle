using System;

namespace GoodNoodle.Application.ViewModel.Star;

public class StarCreateViewModel
{
    public Guid UserId { get; set; }
    public Guid GroupId { get; set; }
    public string Reason { get; set; }
}
