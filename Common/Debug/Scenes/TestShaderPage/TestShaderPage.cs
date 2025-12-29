using Godot;
using System;

public partial class TestShaderPage : Node
{
    [Export]
    public TextureRect BlinkTexture;
    
    [Export]
    public TextureRect OutlineTexture;

    public override void _Ready() {
        base._Ready();

    }
}
