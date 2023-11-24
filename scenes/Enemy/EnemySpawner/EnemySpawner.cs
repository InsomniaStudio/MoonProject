using Godot;
using System;
using System.Diagnostics;

public class EnemySpawner : Spatial
{
	[Export]
	float RangeUp;
	[Export]
	float RangeDown;
	[Export]
	int scalingPoint;
	[Export]
	int scalingValue;
	[Export]
	public PackedScene enemyScene;
	public int enemyCounter = 0;
	Timer timer;
	RandomNumberGenerator random;
	public override void _Ready()
	{
		random = new RandomNumberGenerator();
		timer = GetNode<Timer>("Timer");
	}

	public void _on_Timer_timeout()
	{
		random.Randomize();
		if(enemyCounter < 3)
		{
			Enemy enemy = (Enemy)enemyScene.Instance();
			enemy.scalingPoint = scalingPoint;
			enemy.scalingValue = scalingValue;
			enemy.enemySpawner = this;
			GetParent().AddChild(enemy);
			enemy.Translation = new Vector3(this.Translation.x, 0.5f, this.Translation.z);
			enemyCounter++;
		}
		Translation = new Vector3(this.Translation.x, 0.5f, random.Randf()*(RangeUp-RangeDown)-RangeDown);
		timer.Start();
	}
}
