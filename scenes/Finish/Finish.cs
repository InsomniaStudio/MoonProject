using Godot;
using System;

public class Finish : Area
{
    [Export]
    PackedScene nextLevel;

    public void _on_Finish_body_entered(Node body)
    {
        if(body.GetType() == typeof(Player))
        {
            Player player = (Player)body;
            if(player.scalingPoint == 3) GetTree().ChangeSceneTo(nextLevel);
        }
    }
}
