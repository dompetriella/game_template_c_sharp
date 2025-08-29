using Godot;
using System;

public abstract partial class Entity : CharacterBody2D
{

    public float MoveSpeed;

    public override void _Ready()
    {
        base._Ready();
    }
}