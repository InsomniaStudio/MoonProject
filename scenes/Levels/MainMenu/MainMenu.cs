using Godot;
using System;

public class MainMenu : Control
{
    public Resource saveGame;
    public Resource mouseSens;
    AudioStreamPlayer musicPlayer;
    Label mouseLabel;
    double menuSensitivity;
    
    string[] levels = {"res://scenes/Levels/Level1/Level1_1.tscn",
                        "res://scenes/Levels/Level1/Level1_2.tscn",
                        "res://scenes/Levels/Level1/Level1_3.tscn",
                        "res://scenes/Levels/Level2/Level2_1.tscn",
                        "res://scenes/Levels/Level2/Level2_2.tscn",
                        "res://scenes/Levels/Level2/Level2_3.tscn",
                        "res://scenes/Levels/Level3/Level3_1.tscn",
                        "res://scenes/Levels/Level3/Level3_2.tscn",
                        "res://scenes/Levels/Level3/Level3_3.tscn",
                        "res://scenes/Levels/Level4/Level4.tscn",
                        "res://scenes/Levels/Ending/Ending.tscn"};

    public override void _Ready()
    {
        saveGame = ResourceLoader.Load("res://resources/LevelStats.tres");
        mouseSens = ResourceLoader.Load("res://resources/MouseSens.tres");
        mouseLabel = GetNode<Label>("MouseSens");
        Input.MouseMode = Input.MouseModeEnum.Visible;
        musicPlayer = GetParent().GetNode<AudioStreamPlayer>("MusicPlayer");
        musicPlayer.Play();
        menuSensitivity = 0.05;
        mouseLabel.Text = "Mouse sensitivity: " + menuSensitivity;
    }

    public void _on_NewGame_pressed()
    {   
        if(mouseSens is MouseSens sens)
        {
            sens.sensitivity = menuSensitivity;
        }    
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
        if(mouseSens is MouseSens sens)
        {
            sens.sensitivity = menuSensitivity;
        } 
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

    public void _on_Plus_pressed()
    {
        if(menuSensitivity < 0.3)
            menuSensitivity += 0.05;
        mouseLabel.Text = "Mouse sensitivity: " + menuSensitivity;
    }

    public void _on_Minus_pressed()
    {

        if(menuSensitivity>0)
            menuSensitivity -= 0.05;
        mouseLabel.Text = "Mouse sensitivity: " + menuSensitivity;
    }
}
