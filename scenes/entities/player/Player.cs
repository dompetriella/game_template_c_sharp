using Godot;
using System;

public partial class Player : Entity
{
    [Export] public float speed = 300;
    public override void _Ready()
    {
        base._Ready();

        MoveSpeed = speed;

    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        MoveAndSlide();
    }
}