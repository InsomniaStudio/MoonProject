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

    public STATE state;
    public Camera camera;
    Tool tool;
    Vector3 moveVector;
    int scalingPoint;
    int scalingProgress;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        state = STATE.MOVING;
        scalingPoint = 1;
        scalingProgress = 0;
        camera = this.GetNode<Camera>("Camera");
        tool = camera.GetNode<Tool>("Hook");
        moveVector = new Vector3(0.0f, 0.0f, 0.0f);
        Input.MouseMode = Input.MouseModeEnum.Captured;
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
            this.Scale *= 1.0f+scalingPoint/3.0f;
            scalingPoint++;
            scalingProgress=0;
        }
    }

}
