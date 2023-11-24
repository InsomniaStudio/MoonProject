using Godot;
using System;

public class Player : KinematicBody
{
	public enum STATE {
		MOVING
	};

	[Export]
	public int speed;
	[Export]
	public float mouseSens;

	Resource levelStats;
	
	public STATE state;
	public Camera camera;
	Tool tool;
	Hook hook;
	Hammer hammer;
	public Vector3 moveVector;
	public int scalingPoint;
	int scalingProgress;
	
	public override void _Ready()
	{
		levelStats = GD.Load("res://resources/LevelStats.tres");
		if (levelStats is LevelStats stats)
		{
			GD.Print(stats.level);
		}
		state = STATE.MOVING;
		scalingPoint = 0;
		scalingProgress = 0;
		camera = this.GetNode<Camera>("Camera");
		hook = camera.GetNode<Hook>("Hook");
		hammer = camera.GetNode<Hammer>("Hammer");
		moveVector = new Vector3(0.0f, 0.0f, 0.0f);
		Input.MouseMode = Input.MouseModeEnum.Captured;
		hook.visibility(true);
		hook.visibility(false);
		tool = hook;
	}

	public override void _Process(float delta)
	{
		if(Input.IsActionPressed("ui_cancel"))
			GetTree().Quit();
	}

	public override void _PhysicsProcess(float delta)
	{
		switch (state)
		{
			case STATE.MOVING:
				move_state();
				break;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventMouseMotion mouseMotion)
		{
			camera.RotateX(Mathf.Deg2Rad(mouseMotion.Relative.y*mouseSens*-1));
			camera.RotationDegrees = new Vector3(Mathf.Clamp(camera.RotationDegrees.x, -75.0f, 75.0f), 0.0f, 0.0f);
			this.RotateY(Mathf.Deg2Rad(mouseMotion.Relative.x*mouseSens*-1));
		}
	}

	void move_state()
	{
		moveVector = new Vector3(0.0f, 0.0f, 0.0f);
		if(Input.IsActionPressed("ui_up"))
		{
			moveVector -= camera.GlobalTransform.basis.z;
			moveVector.y = 0.0f;
		}
		if(Input.IsActionPressed("ui_down"))
		{
			moveVector += camera.GlobalTransform.basis.z;
			moveVector.y = 0.0f;
		}
		if(Input.IsActionPressed("ui_left"))
			moveVector -= camera.GlobalTransform.basis.x;
		if(Input.IsActionPressed("ui_right"))
			moveVector += camera.GlobalTransform.basis.x;
		moveVector = moveVector.Normalized();

		if(Input.IsActionJustPressed("ui_accept"))
			tool.shoot();
		tool.select();

		if(Input.IsActionJustPressed("ui_page_up"))
		{
			tool = hook;
			hook.visibility(true);
			hammer.visibility(false);
		}

		if(Input.IsActionJustPressed("ui_page_down"))
		{
			tool = hammer;
			hook.visibility(false);
			hammer.visibility(true);
		}

		MoveAndSlide(moveVector*speed);
	}

	public bool checkScale(int enemyScalingPoint, int enemyScalingValue)
	{
		if(scalingPoint >= enemyScalingPoint)
		{
			scalingProgress += enemyScalingValue;
			if(scalingProgress>=100)
				scale();
			return true;
		}
		else
		{
			return false;
		}
	}
	void scale()
	{
		if(scalingPoint < 3)
		{
			scalingPoint++;
			this.Scale *= 1.0f+0.35f;
			scalingProgress=0;
			hook.upgrade();
			hammer.upgrade();
		}
	}

}
