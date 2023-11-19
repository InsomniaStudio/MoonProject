using Godot;
using System;

public class Hook : Tool
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	RayCast raycast;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		raycast = GetNode<RayCast>("RayCast");
		raycast.Enabled = true;
		raycast.CastTo = new Vector3(0.0f, 0.0f, -100.0f);
	}

	public override void shoot()
	{
		if(raycast.IsColliding() && ((Node)raycast.GetCollider()).GetType() == typeof(Enemy))
		{
			Enemy enemy = (Enemy)raycast.GetCollider();
			enemy.Translation = new Vector3(this.GlobalTranslation.x, enemy.Translation.y, this.GlobalTranslation.z);
			GD.Print(this.GlobalTranslation);
		}
	}
}
