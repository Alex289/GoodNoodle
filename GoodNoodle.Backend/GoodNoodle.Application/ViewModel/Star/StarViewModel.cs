using System;

namespace GoodNoodle.Application.ViewModel.Star;

public class StarViewModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid GroupId { get; set; }
    public string Reason { get; set; }
}
