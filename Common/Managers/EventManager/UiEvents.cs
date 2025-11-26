using Godot;
using System;

public partial class UiEvents : Node
{
    public static UiEvents Instance;

    [Signal]
    public delegate void ShowNotificationEventHandler();


    public override void _Ready()
    {
        base._Ready();

        Instance = this;
    }
}
