using Godot;
using System;

public class Enemy : KinematicBody
{
    [Export]
    public int scalingPoint;
    public override void _Ready()
    {
        this.Scale *= 1+scalingPoint/3.0f;
        Translation += new Vector3(0.0f, scalingPoint/10.0f, 0.0f);
    }

    public override void _PhysicsProcess(float delta)
    {
        
    }

    public void _on_TriggerArea_body_entered(KinematicBody body)
    {
        if(body.GetType() == typeof(Player))
        {
            Player player = (Player)body;
            if(player.checkScale(scalingPoint))
                QueueFree();
        }
    }
}
