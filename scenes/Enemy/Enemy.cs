using Godot;
using System;

public class Enemy : KinematicBody
{
	public enum STATE {
		MOVING,
		ATTACKING,
		HOOKED
	};
	[Export]
	public int scalingPoint;
	[Export]
	public int scalingValue;
	public Vector3 moveVector;
	public STATE state;
	float speed;
	public override void _Ready()
	{
		this.Scale *= 1+scalingPoint/3.0f;
		speed = 100.0f;
		state = STATE.MOVING;
	}

	public override void _PhysicsProcess(float delta)
	{
		GD.Print(state);
		switch (state)
		{
			case STATE.MOVING:
				moveVector = new Vector3(0.0f, 0.0f, 0.0f);	
				break;
			case STATE.ATTACKING:
				Player player = this.GetParent().GetNode<Player>("Player");
				if (player != null) 
				{
					moveVector = player.Transform.origin;
				}
				break;
			case STATE.HOOKED:
				break;
		}
		MoveAndSlide(moveVector);
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
