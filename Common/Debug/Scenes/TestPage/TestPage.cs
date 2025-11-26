using Godot;
using System;
using System.Threading.Tasks;

public partial class TestPage : Control
{
    [Export]
    public Button ReturnButton;

    [Export]
    public NavigationRouteComponent NavigationRouteComponent;

    [Export]
    public Button CounterButton;

    [Export]
    public ProgressBar CounterProgress;

    [Export]
    public Label CounterLabel;



    public override void _Ready()
    {
        base._Ready();


        ReturnButton.Pressed += () =>
        {
            ScaffoldManager.Instance.ScaffoldNewSceneTree(NavigationRouteComponent.Scene.Instantiate());
        };


        // Null checking for testing purposes
        // Example of using the StatefulData 
        if (CounterButton != null && CounterLabel != null && CounterProgress != null)
        {
            var initialValue = UiState.Instance.TestCounter.Value;
            CounterLabel.Text = initialValue.ToString();
            CounterProgress.MaxValue = 100;
            CounterProgress.Value = initialValue;

            CounterButton.Pressed += () =>
            {
                var currentValue = UiState.Instance.TestCounter.Value;
                UiState.Instance.TestCounter.SetValue(currentValue + 5);
            };

            UiState.Instance.TestCounter.ValueChanged += (previousValue, newValue) =>
            {
                CounterLabel.Text = newValue.ToString();
                CreateTween().TweenProperty(CounterProgress, property: ProgressBarProperties.Value, finalVal: newValue, duration: 0.25);

            };
        }

    }
}
