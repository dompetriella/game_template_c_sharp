using Godot;
using System;

public partial class Idle : State
{
    [Export] public Entity entity;

    public override void Update(double delta)
    {
        base.Update(delta);

        if (entity.Velocity != Vector2.Zero)
        {
            EmitSignal(State.SignalName.TransitionState, PlayerStateMachine.States.Moving);
        }

    }

    public override void PhysicsUpdate(double delta)
    {
        base.PhysicsUpdate(delta);

        var inputVector = PlayerStateMachine.GetInputVector();
        entity.Velocity = inputVector * entity.MoveSpeed;
    }
}