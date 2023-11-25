using Godot;
using System;

public class Hammer : Tool
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
		sprite = GetNode<CanvasLayer>("CanvasLayer").GetNode<Sprite>("Sprite");
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
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
		animPlayer.Play("hammer_shoot");
		if(raycast.IsColliding() && ((Node)raycast.GetCollider()).GetType() == typeof(Enemy))
		{
//			Enemy enemy = (Enemy)enemyScene.Instance();
//			enemy.scalingPoint = scalingPoint;
//			enemy.scalingValue = scalingValue;
//			enemy.enemySpawner = this;
			Enemy enemy = (Enemy)raycast.GetCollider();
			Enemy newEnemy1 = new Enemy(enemy);
			Enemy newEnemy2 = new Enemy(enemy);
			GetParent().AddChild(newEnemy1);
			GetParent().AddChild(newEnemy2);
			newEnemy1.Translation = enemy.Translation + new Vector3(3.0f, 0.0f, 0.0f);
			newEnemy2.Translation = enemy.Translation + new Vector3(-3.0f, 0.0f, 0.0f);
			enemy.scaleBack(1);
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
