using Godot;
using System;
using System.Threading.Tasks;

public partial class AudioManager : Node
{
    public static AudioManager Instance { get; private set; }

    [Export]
    private AudioStreamPlayer MusicNode;


    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }

    public void PlayMusic(AudioStream audioStream, float fadeInTime = 0.0f)
    {
        if (MusicNode.Playing)
        {
            MusicNode.Stop();

            MusicNode.VolumeDb = 0;
        }

        MusicNode.Stream = audioStream;

        if (fadeInTime > 0.0f)
        {
            MusicNode.VolumeDb = -80;
            MusicNode.Play();
            Tween musicTween = GetTree().CreateTween();
            musicTween.TweenProperty(MusicNode, AudioStreamPlayerProperties.VolumeDb, 0, fadeInTime);
        }
        else
        {
            MusicNode.VolumeDb = 0;
            MusicNode.Play();
        }

    }


    public async void PauseMusic(float fadeOutTime = 0.0f)
    {
        if (!MusicNode.Playing)
        {
            GD.PushWarning("Cannot pause music - no music playing");
            return;
        }

        if (fadeOutTime > 0)
        {
            Tween musicTween = GetTree().CreateTween();
            musicTween.TweenProperty(MusicNode, AudioStreamPlayerProperties.VolumeDb, -80, fadeOutTime);

            await ToSignal(musicTween, Tween.SignalName.Finished);
        }

        MusicNode.StreamPaused = true;
    }

    public async Task PlaySoundEffect(AudioStream soundEffect)
    {
        AudioStreamPlayer audioStreamPlayer = new AudioStreamPlayer();
        AddChild(audioStreamPlayer);

        audioStreamPlayer.Stream = soundEffect;
        audioStreamPlayer.Play();

        await ToSignal(audioStreamPlayer, AudioStreamPlayer.SignalName.Finished);

        audioStreamPlayer.QueueFree();
    }
}
