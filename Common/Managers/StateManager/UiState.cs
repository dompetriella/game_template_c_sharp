using Godot;
using System;
using System.Net.NetworkInformation;

public partial class UiState : Node
{
    public static UiState Instance;

    public StatefulData<int> TestCounter = new(0);

    public int Testvalue = 5;


    public override void _Ready()
    {
        base._Ready();

        Instance = this;
    }


}
