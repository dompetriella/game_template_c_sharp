using Godot;
using System;

public partial class TransitionManager : CanvasLayer
{

    [Signal]
    public delegate void TransitionStartedEventHandler(string animationName);

    [Signal]
    public delegate void TransitionEndedEventHandler(string animationName);

    public static TransitionManager Instance { get; private set; }


    [Export]
    private ColorRect ColorRectNode;

    [Export]
    private AnimationPlayer AnimationPlayerNode;


    public static class TransitionType
    {
        public const string FadeIn = "fade_in";
        public const string FadeOut = "fade_out";
    }

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }

    public void PlayTransition(string transitionType, double transitionDuration = 1.0, bool shouldDisableInputOnStart = true, bool shouldEnableInputOnEnd = true)
    {
        Animation animation = AnimationPlayerNode.GetAnimation(transitionType);

        if (animation == null)
        {
            GD.PushWarning($"Animation with name {transitionType} does not exist, returning");
            return;
        }

        float animationLength = animation.Length;

        if (!(animationLength > 0f))
        {
            GD.PushWarning($"Animation with name {transitionType} does not have a length, returning");
            return;
        }

        float customSpeed = (float)(animationLength / transitionDuration);


        AnimationPlayerNode.Play(name: transitionType, customSpeed: customSpeed);

        AnimationPlayerNode.AnimationStarted += (animationName) => OnAnimationStarted(animationName: animationName, shouldDisableInputOnStart: shouldDisableInputOnStart);
        AnimationPlayerNode.AnimationFinished += (animationName) => OnAnimationFinished(animationName: animationName, shouldEnableInputOnEnd: shouldEnableInputOnEnd);
    }

    private void OnAnimationStarted(StringName animationName, bool shouldDisableInputOnStart)
    {
        if (shouldDisableInputOnStart)
        {
            Layer = 99;
        }
        EmitSignal(signal: SignalName.TransitionStarted, animationName);
    }

    private void OnAnimationFinished(StringName animationName, bool shouldEnableInputOnEnd)
    {
        if (shouldEnableInputOnEnd)
        {
            Layer = 0;
        }
        EmitSignal(signal: SignalName.TransitionEnded, animationName);
    }
}
