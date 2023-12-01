using Godot;
using System;

public class Hammer : Tool
{
	Resource toolStats;
	
	RayCast raycast;
	AnimationPlayer animPlayer;
	public AnimationPlayer animPlayer2;
	AudioStreamPlayer audioPlayer;
	Enemy enemy;
	Sprite sprite;
	
	public override void _Ready()
	{
		raycast = GetNode<RayCast>("RayCast");
		sprite = GetNode<CanvasLayer>("CanvasLayer").GetNode<Sprite>("Sprite");
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animPlayer2 = GetNode<AnimationPlayer>("AnimationPlayer2");
		audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
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
		audioPlayer.Play();
		if(raycast.IsColliding() && ((Node)raycast.GetCollider()).GetType() == typeof(Enemy))
		{
			enemy.scaleBack(1);
			// enemy.hammerDamage.Emitting = true;
			// enemy.hammerDamage.OneShot = true;
			if (enemy.slimeEnemy) enemy.halfed();
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

	public override void move(bool status)
	{
		if(status)
			animPlayer2.Play("moving");
		else
			animPlayer2.Stop();
	}
}
