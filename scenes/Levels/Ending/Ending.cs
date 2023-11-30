using Godot;
using System;

public class Ending : Spatial
{
    AnimationPlayer animPlayer;
    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animPlayer.Play("ending");
    }

    public void _on_AnimationPlayer_animation_finished(String animName)
    {
        GetTree().ChangeSceneTo((PackedScene)ResourceLoader.Load("res://scenes/Levels/MainMenu/MainMenu.tscn"));
    }
}

