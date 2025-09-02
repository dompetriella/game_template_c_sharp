using Godot;
using System;

public partial class ScaffoldManager : Node
{
    public static ScaffoldManager Instance { get; private set; }

    public override void _Ready()
    {
        base._Ready();

        Instance = this;
    }
}
