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
