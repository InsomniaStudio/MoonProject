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
        levelStats = GD.Load("res://resources/LevelStats.tres");
        if(levelStats is LevelStats stats)
                GD.Print(stats.level);
    }
    public void _on_Finish_body_entered(Node body)
    {
        if(body.GetType() == typeof(Player))
        {
            Player player = (Player)body;
            if(player.scalingPoint >= 3) 
            {
                if(levelStats is LevelStats stats)
                    stats.level++;
                GetTree().ChangeSceneTo(nextLevel);
            }
        }
    }
}
