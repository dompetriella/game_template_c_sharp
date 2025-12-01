using Godot;
using System;
using System.Threading.Tasks;

public partial class NotificationManager : Node
{

    public static NotificationManager Instance;

    [Signal]
    public delegate void NotificationShownEventHandler(string messageText);

    [Signal]
    public delegate void NotificationRemovedEventHandler(string messageText);


    [Export]
    public VBoxContainer NotificationColumn;

    public override void _Ready()
    {
        base._Ready();

        Instance = this;
    }

    public async Task ShowNotification(string messageText, double showDurationInMs = 5000, double fadeInDurationInMs = 500, double fadeOutDurationInMs = 500)
    {
        var notificationInstance = ResourceLoader.Load<PackedScene>("uid://ciudsaf8ox1g4").Instantiate<Notification>();
        notificationInstance.Message = messageText;
        notificationInstance.Modulate = new Color(1, 1, 1, 0);
        NotificationColumn.AddChild(notificationInstance);

        EmitSignal(SignalName.NotificationShown);

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        var tweenIn = CreateTween().SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.In);
        tweenIn.TweenProperty(notificationInstance, ControlProperties.Modulate.Alpha, 1.0, duration: fadeInDurationInMs / 1000);
        await ToSignal(tweenIn, Tween.SignalName.Finished);

        var timer = GetTree().CreateTimer(timeSec: showDurationInMs / 1000);
        await ToSignal(timer, Timer.SignalName.Timeout);

        var tweenOut = CreateTween().SetEase(Tween.EaseType.InOut);
        tweenOut.TweenProperty(notificationInstance, ControlProperties.Modulate.Alpha, 0.0, duration: fadeOutDurationInMs / 1000);
        await ToSignal(tweenOut, Tween.SignalName.Finished);

        notificationInstance.QueueFree();

        EmitSignal(SignalName.NotificationRemoved);
    }
}
