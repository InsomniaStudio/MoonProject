using Godot;
using System;

public class Finish : Area
{
    [Export]
    PackedScene nextLevel;
    public Resource levelStats;
    AnimationPlayer animPlayer;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer.Play("enter");
        if(levelStats is LevelStats stats)
                GD.Print(stats.level);
    }
    public void _on_Finish_body_entered(Node body)
    {
        if(body.GetType() == typeof(Player))
        {
            Player player = (Player)body;
            if(levelStats is LevelStats stats)
                stats.level++;
            if(player.scalingPoint == 3) GetTree().ChangeSceneTo(nextLevel);
        }
    }
}
