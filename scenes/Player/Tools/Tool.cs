using Godot;
using System;
using System.Runtime.InteropServices;

public class Tool : Spatial
{
	[Export]
	public Resource toolStats;
	
	public int cooldown;
	public override void _Ready()
	{
		if (toolStats is ToolStats stats) 
		{
			cooldown = stats.cooldown;
			GD.Print(stats.cooldown);
		}
	}

	public virtual void shoot()
	{

	}

	public virtual void select() {}

	public void upgrade()
	{
		cooldown -= 5;
		if(toolStats is ToolStats stats)
		{
			stats.cooldown = cooldown;
			GD.Print(stats.cooldown);
		}
	}
}
