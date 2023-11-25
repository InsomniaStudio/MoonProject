using Godot;
using System;

public class Hook : Tool
{
	Resource toolStats;
	RayCast raycast;
	AnimationPlayer animPlayer;
	Enemy enemy;
	Sprite sprite;
	
	public override void _Ready()
	{
		toolStats = GD.Load("res://resources/ToolStats.tres");
		if (toolStats is ToolStats stats)
		{
			GD.Print(stats.cooldown);
		}
		raycast = GetNode<RayCast>("RayCast");
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		sprite = GetNode<CanvasLayer>("CanvasLayer").GetNode<Sprite>("Sprite");
		raycast.Enabled = true;
		raycast.CastTo = new Vector3(0.0f, 0.0f, -100.0f);
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
		animPlayer.Play("hook_shoot");
		if(raycast.IsColliding() && (((Node)raycast.GetCollider()).GetType() == typeof(Enemy)))
		{
			Enemy enemy = (Enemy)raycast.GetCollider();
			if(!enemy.stoneEnemy)
				enemy.Translation = new Vector3(this.GlobalTranslation.x, enemy.Translation.y, this.GlobalTranslation.z);
			GD.Print(this.Name);
		}
	}
	
	public override void upgrade()
	{
		toolStats = GD.Load("res://resources/ToolStats.tres");
		if (toolStats is ToolStats stats)
		{
			stats.cooldown -= 5;
			GD.Print(stats.cooldown);
		}
	}
}
