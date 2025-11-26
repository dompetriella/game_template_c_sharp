using Godot;
using System;

public partial class Notification : MarginContainer
{
    public string Message;

    [Export]
    public RichTextLabel MessageLabel;

    public override void _Ready() {
        base._Ready();

        MessageLabel.Text = Message;
    }

}
