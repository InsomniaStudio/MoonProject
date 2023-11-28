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
	public PackedScene classicScene;
	[Export]
	public PackedScene stoneScene;
	[Export]
	public PackedScene slimeScene;
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
		uint tmp = random.Randi()%3+1;
		GD.Print(tmp);
		if(enemyCounter < 3)
		{
			switch (tmp)
			{
				case 1 :
				{
					Enemy enemy = (Enemy)classicScene.Instance();
					enemy.scalingPoint = scalingPoint;
					enemy.scalingValue = scalingValue;
					enemy.enemySpawner = this;
					GetParent().AddChild(enemy);
					enemy.Translation = new Vector3(this.Translation.x, 0.5f, this.Translation.z);
					enemyCounter++;
					break;
				}
				case 2 :
				{
					Enemy enemy = (Enemy)stoneScene.Instance();
					enemy.scalingPoint = scalingPoint;
					enemy.scalingValue = scalingValue;
					enemy.enemySpawner = this;
					GetParent().AddChild(enemy);
					enemy.Translation = new Vector3(this.Translation.x, 0.5f, this.Translation.z);
					enemyCounter++;
					break;
				}
				case 3:
				{
					Enemy enemy = (Enemy)slimeScene.Instance();
					enemy.scalingPoint = scalingPoint;
					enemy.scalingValue = scalingValue;
					enemy.enemySpawner = this;
					GetParent().AddChild(enemy);
					enemy.Translation = new Vector3(this.Translation.x, 0.5f, this.Translation.z);
					enemyCounter++;
					break;
				}
			}
			
		}
		Translation = new Vector3(this.Translation.x, 0.5f, random.RandfRange(RangeDown, RangeDown));
		timer.Start();
	}
}
