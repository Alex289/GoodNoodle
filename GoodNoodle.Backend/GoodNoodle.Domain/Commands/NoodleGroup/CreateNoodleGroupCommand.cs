using System;

namespace GoodNoodle.Domain.Commands.NoodleGroup;

public class CreateNoodleGroupCommand : Command
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }

    public CreateNoodleGroupCommand(Guid id, string name, string image)
    {
        Id = id;
        Name = name;
        Image = image;
    }
}
