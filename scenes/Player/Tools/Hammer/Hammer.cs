using Godot;
using System;

public class Hammer : Tool
{
    RayCast raycast;
	// AnimationPlayer animPlayer;
	Enemy enemy;
    Sprite sprite;
	
	public override void _Ready()
	{
		raycast = GetNode<RayCast>("RayCast");
        sprite = GetNode<CanvasLayer>("CanvasLayer").GetNode<Sprite>("Sprite");
		// animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		raycast.Enabled = true;
		raycast.CastTo = new Vector3(0.0f, 0.0f, -5.0f);
	}

    public override void _Process(float delta)
    {
        if(raycast.IsColliding() && ((Node)raycast.GetCollider()).GetType() == typeof(Enemy))
		{
			enemy = (Enemy)raycast.GetCollider();
			enemy.selected = true;
		}
		else if(enemy != null)
		{
			enemy.selected = false;
		}
    }
    
    public void visibility(bool value)
	{
		sprite.Visible = value;
	}
	public override void shoot()
	{
		// animPlayer.Play("hook_shoot");
		if(raycast.IsColliding() && ((Node)raycast.GetCollider()).GetType() == typeof(Enemy))
		{
			Enemy enemy = (Enemy)raycast.GetCollider();
            enemy.scaleBack(1);
			GD.Print(this.Name);
		}
	}
}