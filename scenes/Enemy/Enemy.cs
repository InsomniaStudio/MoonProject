using Godot;
using System;
using System.Net.Sockets;

public class Enemy : KinematicBody
{
	public enum STATE {
		MOVING,
		ATTACKING,
		// TODO: define evade logic for smaller Enemies 
		EVADING,
		HOOKED
	};
	[Export]
	public int scalingPoint;
	[Export]
	public int scalingValue;
	public Vector3 moveVector;
	public STATE state;
	public bool selected;
	float speed;
	Sprite3D sprite;
	public override void _Ready()
	{
		this.Scale *= 1+scalingPoint*0.25f;
		speed = 100.0f;
		selected = false;
		state = STATE.MOVING;
		sprite = GetNode<Sprite3D>("Sprite3D");
	}

	public override void _PhysicsProcess(float delta)
	{
//		GD.Print(state);
		switch (state)
		{
			case STATE.MOVING:
				moveVector = new Vector3(0.0f, 0.0f, 0.0f);
				select();	
				break;
			case STATE.ATTACKING:
				Player player = this.GetParent().GetNode<Player>("Player");
				if (player != null) 
				{
					moveVector = player.Translation - this.Translation;
					moveVector = moveVector.Normalized();
				}
				break;
			case STATE.HOOKED:
				break;
		}
		MoveAndSlide(moveVector);
	}
	
	public void scaleBack(int value)
	{
		scalingPoint -= value;
		if(scalingPoint<0)
			scalingPoint = 0;
		else
			this.Scale *= 0.75f;
	}
	public void select()
	{
		if(selected)
			sprite.Modulate = new Color(0.67f, 0.67f, 0.67f, 1.0f);
		else
			sprite.Modulate = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	}

	private void _on_TriggerArea_body_entered(object body)
	{
		if(body.GetType() == typeof(Player))
		{
			Player player = (Player)body;
			if (player != null) 
			{
				state = STATE.ATTACKING;
			}
			if(player.checkScale(scalingPoint, scalingValue))
				QueueFree();
		}
	}

	private void _on_TriggerArea_body_exited(object body)
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
}
