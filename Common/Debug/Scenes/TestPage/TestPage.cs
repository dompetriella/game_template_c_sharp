using Godot;
using System;
using System.Threading.Tasks;

public partial class TestPage : Control
{
    [Export]
    public Button ReturnButton;

    public override void _Ready()
    {
        base._Ready();

        PackedScene previousPage = ResourceLoader.Load<PackedScene>("uid://xod7cm3vmepr");


        ReturnButton.Pressed += () =>
        {
            ScaffoldManager.Instance.ScaffoldNewSceneTree(previousPage.Instantiate());
        };

    }
}
