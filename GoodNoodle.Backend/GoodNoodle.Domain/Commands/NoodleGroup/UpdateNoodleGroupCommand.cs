using System;

namespace GoodNoodle.Domain.Commands.NoodleGroup;

public class UpdateNoodleGroupCommand : Command
{
    public Guid Id;
    public string Name;
    public string Image;

    public UpdateNoodleGroupCommand(Guid id, string name, string image)
    {
        Id = id;
        Name = name;
        Image = image;
    }
}
