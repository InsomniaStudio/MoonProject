using Godot;
using System;
using System.Runtime.InteropServices;

public class Tool : Spatial
{
	// TODO: define more stats for tools
	public int cooldown;
	public override void _Ready()
	{
	}

	public virtual void shoot()
	{

	}

	public virtual void select() {}

	// TODO: define upgrade for specific tool (Hook/Hammer)
	public void upgrade()
	{
	}
}
