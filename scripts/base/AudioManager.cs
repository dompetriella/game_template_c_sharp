using Godot;
using System;

public partial class AudioManager : Node
{
    public static AudioManager Instance { get; private set; }
    private AudioStreamPlayer PlayerSFX;
    private AudioStreamPlayer EnemySFX;
    private AudioStreamPlayer WorldSFX;
    private AudioStreamPlayer UserInterfaceSFX;
    private AudioStreamPlayer Music;

    public enum SoundEffectSource
    {

        Player,
        Enemy,
        World,
        UI,

    }

    public override void _Ready()
    {
        base._Ready();
        Instance = this;

        PlayerSFX = GetNode<AudioStreamPlayer>("%PlayerSFX");
        EnemySFX = GetNode<AudioStreamPlayer>("%EnemySFX");
        WorldSFX = GetNode<AudioStreamPlayer>("%WorldSFX");
        UserInterfaceSFX = GetNode<AudioStreamPlayer>("%UserInterfaceSFX");
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

    public void PlaySoundEffect(AudioStream soundEffect, SoundEffectSource soundEffectSource)
    {

        AudioStreamPlayer audioPlayer = (soundEffectSource) switch
        {
            SoundEffectSource.Player => PlayerSFX,
            SoundEffectSource.Enemy => EnemySFX,
            SoundEffectSource.World => WorldSFX,
            SoundEffectSource.UI => UserInterfaceSFX,
            _ => throw new ArgumentOutOfRangeException(nameof(soundEffectSource), soundEffectSource, null)
        };

        audioPlayer.Stream = soundEffect;
        audioPlayer.Play();

    }
}
