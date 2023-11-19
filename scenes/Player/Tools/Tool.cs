using Godot;
using System;

public class Tool : Spatial
{
	[Export]
	public Resource toolStats;
	
	public int cooldown;
	public override void _Ready()
	{
		if (toolStats is ToolStats stats) 
		{
			GD.Print(stats == null);
		}
	}

	public virtual void shoot()
	{

	}
}
