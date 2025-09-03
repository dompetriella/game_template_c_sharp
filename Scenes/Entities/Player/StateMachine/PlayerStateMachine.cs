using Godot;
using System;

public partial class PlayerStateMachine : StateMachine
{
    public static class States
    {
        public const string Immobile = "Immobile";
        public const string Moving = "Moving";
        public const string Idle = "Idle";
    }

    public static Vector2 GetInputVector()
    {
        var inputVector = Vector2.Zero;

        if (Input.IsActionPressed(InputActions.UiRight)) inputVector.X += 1;

        if (Input.IsActionPressed(InputActions.UiLeft)) inputVector.X -= 1;

        if (Input.IsActionPressed(InputActions.UiUp)) inputVector.Y -= 1;

        if (Input.IsActionPressed(InputActions.UiDown)) inputVector.Y += 1;

        inputVector = inputVector.Normalized();

        return inputVector;
    }
}