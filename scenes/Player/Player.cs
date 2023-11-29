using Godot;
using System;
using System.IO;
using System.Runtime.CompilerServices;

public class Player : KinematicBody
{
	public enum STATE {
		MOVING
	};

	[Export]
	public int speed;
	[Export]
	public float mouseSens;
	[Export]
	public bool haveHook;
	[Export]
	public bool haveHammer;
	public Resource levelStats;
	
	public STATE state;
	public Camera camera;
	Tool tool;
	Hook hook;
	Hammer hammer;
	ColorRect colorRect;
	ColorRect colorRect2;
	ColorRect colorRect4;
	ProgressBar progressBar;
	Timer colorTimer;
	AudioStreamPlayer pickUp;
	AudioStreamPlayer damageSound;
	AudioStreamPlayer scaleUp;
	AudioStreamPlayer walkSound;
	public Vector3 moveVector;
	public int scalingPoint;
	int scalingProgress;
	public int health;
	
	public override void _Ready()
	{
		levelStats = GD.Load("res://resources/LevelStats.tres");
		if (levelStats is LevelStats stats)
		{
			GD.Print(stats.level);
		}
		state = STATE.MOVING;
		scalingPoint = 3;
		scalingProgress = 0;
		health = 100;
		camera = this.GetNode<Camera>("Camera");
		hook = camera.GetNode<Hook>("Hook");
		hammer = camera.GetNode<Hammer>("Hammer");
		colorRect = GetNode<CanvasLayer>("CanvasLayer").GetNode<ColorRect>("ColorRect");
		colorRect2 = GetNode<CanvasLayer>("CanvasLayer").GetNode<ColorRect>("ColorRect2");
		colorRect4 = GetNode<CanvasLayer>("CanvasLayer").GetNode<ColorRect>("ColorRect4");
		progressBar = GetNode<CanvasLayer>("CanvasLayer").GetNode<ProgressBar>("ProgressBar");
		colorTimer = GetNode<Spatial>("Timers").GetNode<Timer>("ColorTimer");
		pickUp = GetNode<Spatial>("AudioPlayers").GetNode<AudioStreamPlayer>("PickUp");
		damageSound = GetNode<Spatial>("AudioPlayers").GetNode<AudioStreamPlayer>("Damage");
		scaleUp = GetNode<Spatial>("AudioPlayers").GetNode<AudioStreamPlayer>("ScaleUp");
		walkSound = GetNode<Spatial>("AudioPlayers").GetNode<AudioStreamPlayer>("Walking");
		moveVector = new Vector3(0.0f, 0.0f, 0.0f);
		Input.MouseMode = Input.MouseModeEnum.Captured;
		hook.visibility(false);
		hammer.visibility(false);
		if(haveHook) tool = hook;
		if(haveHammer) tool = hammer;
	}

	public override void _Process(float delta)
	{
		if(Input.IsActionPressed("ui_cancel"))
		{
			saveGame();
			GetTree().ChangeSceneTo((PackedScene)ResourceLoader.Load("res://scenes/Levels/MainMenu/MainMenu.tscn"));
		}
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

		if(Input.IsActionJustPressed("ui_accept") && tool != null)
			tool.shoot();
		if(tool != null)
			tool.select();

		if(Input.IsActionJustPressed("ui_page_up") && haveHook)
		{
			tool = hook;
			hook.visibility(true);
			hammer.visibility(false);
		}

		if(Input.IsActionJustPressed("ui_page_down") && haveHammer)
		{
			tool = hammer;
			hook.visibility(false);
			hammer.visibility(true);
		}

		if(moveVector != new Vector3(0.0f, 0.0f, 0.0f) && !walkSound.Playing)
			walkSound.Play();
		else if(moveVector == new Vector3(0.0f, 0.0f, 0.0f) && walkSound.Playing)
			walkSound.Stop();

		if(moveVector != new Vector3(0.0f, 0.0f, 0.0f) && tool!=null)
			tool.move(true);
		else if(tool!=null)
			tool.move(false);

		progressBar.Value = health;
		MoveAndSlide(moveVector*speed);
	}

	public bool checkScale(int enemyScalingPoint, int enemyScalingValue)
	{
		if(scalingPoint >= enemyScalingPoint)
		{
			scalingProgress += enemyScalingValue;
			if(scalingProgress>=100)
			{
				colorRect2.Visible = true;
				scale();
			}
			else
			{
				pickUp.Play();
				colorRect.Visible = true;
			}
			colorTimer.Start();
			return true;
		}
		else
		{
			return false;
		}
	}

	public void damage(int value)
	{
		health -= value;
		damageSound.Play();
		colorRect4.Visible = true;
		colorTimer.Start();	
		if (health <= 0)
		{
			GetTree().ReloadCurrentScene();
		}
	}
	void scale()
	{
		scaleUp.Play();
		if(scalingPoint < 3)
		{
			scalingPoint++;
			this.Scale *= 1.0f+0.25f;
//			hook.upgrade();
			hammer.upgrade();
		}
		scalingProgress=0;
	}

	void saveGame()
	{
		levelStats = GD.Load("res://resources/LevelStats.tres");
		
		if (levelStats is LevelStats stats)
		{
			GD.Print(stats.level);
			Godot.File namefile = new Godot.File();
			namefile.Open("user://savegame.dat", Godot.File.ModeFlags.Write);
			namefile.Store32((uint)stats.level);
			namefile.Close();
		}
	}

	public void _on_ColorTimer_timeout()
	{
		colorRect.Visible = false;
		colorRect2.Visible = false;
		colorRect4.Visible = false;
	}

}
