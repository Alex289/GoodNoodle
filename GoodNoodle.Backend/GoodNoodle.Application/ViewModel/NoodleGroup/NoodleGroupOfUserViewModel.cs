using System;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Application.ViewModel.NoodleGroup;


public class NoodleGroupOfUserViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public GroupRole GroupRole { get; set; }
}
