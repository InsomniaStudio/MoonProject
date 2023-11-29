using Godot;
using System;

public class MainMenu : Control
{
    public Resource saveGame;
    string[] levels = {"res://scenes/Levels/Level1/Level1_1.tscn",
                        "res://scenes/Levels/Level1/Level1_2.tscn",
                        "res://scenes/Levels/Level1/Level1_3.tscn",
                        "res://scenes/Levels/Level2/Level2_1.tscn",
                        "res://scenes/Levels/Level2/Level2_2.tscn",
                        "res://scenes/Levels/Level2/Level2_3.tscn"};

    public override void _Ready()
    {
        saveGame = ResourceLoader.Load("res://resources/LevelStats.tres");
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public void _on_NewGame_pressed()
    {       
        if(saveGame is LevelStats stats)
        {
            stats.level = 0;
            GD.Print(stats.level);
            PackedScene levelScene = (PackedScene)ResourceLoader.Load(levels[stats.level]);
            GetTree().ChangeSceneTo(levelScene);
        }
    }

    public void _on_Continue_pressed()
    {
        if(saveGame is LevelStats stats)
        {
            GD.Print(stats.level);
            Godot.File namefile = new Godot.File();
			// if((namefile.Open("user://savegame.dat", Godot.File.ModeFlags.Read)).GetType() != typeof(Error))
            // {
			//     uint levelNumber = namefile.Get32();
            //     stats.level = (int)levelNumber;
            // }
            try
            {
                namefile.Open("user://savegame.dat", Godot.File.ModeFlags.Read);
                uint levelNumber = namefile.Get32();
                stats.level = (int)levelNumber;
                namefile.Close();
            }
            catch(Exception e)
            {}
            PackedScene levelScene = (PackedScene)ResourceLoader.Load(levels[stats.level]);
            GetTree().ChangeSceneTo(levelScene);
        }
    }

    public void _on_Exit_pressed()
    {
        GetTree().Quit();
    }
}
