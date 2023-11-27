using Godot;
using System;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

public class Enemy : KinematicBody
{
	public enum STATE {
		MOVING,
		ATTACKING, 
		EVADING,
		HOOKED
	};
	[Export]
	public int scalingPoint;
	[Export]
	public int scalingValue;
	[Export]
	public bool classicEnemy;
	[Export]
	public bool stoneEnemy;
	[Export]
	public bool slimeEnemy;
	public Vector3 moveVector;
	public STATE state;
	public bool selected;
	protected float speed;
	protected Sprite3D sprite;
	Player player;
	public EnemySpawner enemySpawner;
	public Particles hammerDamage;
	AnimationPlayer animPlayer;
	
	public override void _Ready()
	{
		this.Scale *= 1+scalingPoint*0.25f;
		speed = 5.0f;
		selected = false;
		state = STATE.MOVING;
		sprite = GetNode<Sprite3D>("Sprite3D");
		hammerDamage = GetNode<Particles>("Particles");
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		if(stoneEnemy) sprite.Modulate = new Color(0.0f, 0.0f, 1.0f, 1.0f);
		if(slimeEnemy) sprite.Modulate = new Color(1.0f, 0.0f, 0.0f, 1.0f);
		player = this.GetParent().GetNode<Player>("Player");
		moveVector = new Vector3(1.0f, 0.0f, 0.0f);
	}

	public override void _PhysicsProcess(float delta)
	{
		if(!animPlayer.IsPlaying())
			animPlayer.Play("moving");
		switch (state)
		{
			case STATE.MOVING:
				if(this.IsOnWall())
				{
					GD.Print("colision");
					moveVector.x*=-1;
				}
				if(this.Translation.DistanceTo(player.Translation) < 50.0f && scalingPoint>player.scalingPoint)
					state = STATE.ATTACKING;
				select();	
				break;
			case STATE.ATTACKING:
				if (player != null) 
				{
					moveVector = player.Translation - this.Translation;
					moveVector = moveVector.Normalized();
				}
				if(this.Translation.DistanceTo(player.Translation) > 50.0f)
					state = STATE.MOVING;
				select();	
				break;
			case STATE.HOOKED:
				break;
		}
		MoveAndSlide(moveVector*speed);
	}
	
	public void halfed()
	{
		Enemy enemy = (Enemy)(ResourceLoader.Load<PackedScene>("res://scenes/Enemy/ClassicEnemy/ClassicEnemy.tscn").Instance());
		enemy.scalingPoint = scalingPoint;
		enemy.scalingValue = scalingValue;
		enemy.enemySpawner = this.enemySpawner;
		this.enemySpawner.enemyCounter++;
		this.GetParent().AddChild(enemy);
		enemy.Translation = this.Translation + new Vector3(2.0f, 0.0f, 0.0f);
	}
	
	public void scaleBack(int value)
	{
		scalingPoint -= value;
		if(scalingPoint<0)
			scalingPoint = 0;
		else
			this.Scale *= 0.75f;
		animPlayer.Play("hurt");
	}
	public void select()
	{
		if(selected && (scalingPoint<=player.scalingPoint))
			sprite.Modulate = new Color(0.67f, 0.67f, 0.67f, 1.0f);
		else
		{
			sprite.Modulate = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			if(stoneEnemy) sprite.Modulate = new Color(0.0f, 0.0f, 1.0f, 1.0f);
			if(slimeEnemy) sprite.Modulate = new Color(1.0f, 0.0f, 0.0f, 1.0f);
		}
	}

	private void _on_TriggerArea_body_entered(Node body)
	{
		if(body.GetType() == typeof(Player))
		{
			Player player = (Player)body;
			if (!player.checkScale(scalingPoint, scalingValue))
			{
				player.health -= 5;	
				if (player.health == 0)
				{
					this.GetTree().ReloadCurrentScene();
				}
			}
			GD.Print(player.health);
			if (player != null) 
			{
				state = STATE.ATTACKING;
			}
			if (player.checkScale(scalingPoint, scalingValue))
			{
				enemySpawner.enemyCounter--;
				QueueFree();
			}
		}
	}

	private void _on_TriggerArea_body_exited(Node body)
	{
		if(body.GetType() == typeof(Player))
		{
			Player player = (Player)body;
			if (player != null) 
			{
				state = STATE.MOVING;
			}
		}
	}

	public void _on_AnimationPlayer_animation_finished(string animName)
	{
		if(animName == "hurt")
			animPlayer.Play("moving");
	}
}
