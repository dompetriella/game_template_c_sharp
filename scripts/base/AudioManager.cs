using Godot;
using System;
using System.Threading.Tasks;

public partial class AudioManager : Node
{
    public static AudioManager Instance { get; private set; }
    private AudioStreamPlayer Music;


    public override void _Ready()
    {
        base._Ready();
        Instance = this;

        Music = GetNode<AudioStreamPlayer>("%Music");
    }

    public void PlayMusic(AudioStream audioStream, float fadeInTime = 0.0f)
    {
        if (Music.Playing)
        {
            Music.Stop();

            Music.VolumeDb = 0;
        }

        Music.Stream = audioStream;

        if (fadeInTime > 0.0f)
        {
            Music.VolumeDb = -80;
            Music.Play();
            Tween musicTween = GetTree().CreateTween();
            musicTween.TweenProperty(Music, AudioStreamPlayerProperties.VolumeDb, 0, fadeInTime);
        }
        else
        {
            Music.VolumeDb = 0;
            Music.Play();
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
