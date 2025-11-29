using Godot;
using System;
using System.Threading.Tasks;

[GlobalClass]
public partial class CooldownComponent : Node
{
    /// <summary>
    /// Total duration of the cooldown in milliseconds.
    /// </summary>
    [Export] public int CooldownTime { get; set; } = 10000;

    /// <summary>
    /// If true, cooldown will automatically restart after finishing.
    /// </summary>
    [Export] public bool AutoRepeat { get; set; } = false;

    /// <summary>
    /// True if the cooldown is currently running.
    /// </summary>
    public bool IsRunning { get; private set; } = false;

    /// <summary>
    /// Time remaining in milliseconds.
    /// </summary>
    public int TimeRemaining { get; private set; } = 0;

    /// <summary>
    /// Signal emitted when the cooldown starts.
    /// </summary>
    [Signal] public delegate void CooldownStartedEventHandler();

    /// <summary>
    /// Signal emitted when the cooldown finishes.
    /// </summary>
    [Signal] public delegate void CooldownFinishedEventHandler();

    private bool isPaused = false;

    public override void _Process(double delta)
    {
        if (IsRunning && !isPaused)
        {
            TimeRemaining -= (int)(delta * 1000);

            if (TimeRemaining <= 0)
            {
                TimeRemaining = 0;
                IsRunning = false;
                EmitSignal(SignalName.CooldownFinished);

                if (AutoRepeat)
                {
                    Start();
                }
            }
        }
    }

    /// <summary>
    /// Starts or restarts the cooldown.  Emits the CooldownStarted signal
    /// </summary>
    public void Start()
    {
        TimeRemaining = CooldownTime;
        IsRunning = true;
        isPaused = false;

        EmitSignal(SignalName.CooldownStarted);
    }

    public void Stop()
    {
        IsRunning = false;
        TimeRemaining = 0;
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        if (IsRunning)
            isPaused = false;
    }
}
