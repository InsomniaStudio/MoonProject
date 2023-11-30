using Godot;
using System;

public class Level4 : Spatial
{
    AudioStreamPlayer musicPlayer;
    public override void _Ready()
    {
        musicPlayer = GetParent().GetNode<AudioStreamPlayer>("MusicPlayer");
        musicPlayer.Stop();
    }


}
