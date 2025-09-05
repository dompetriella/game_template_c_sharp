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

    public async void StartMusicTrack(AudioStream audioStream, float fadeInTime = 0.0f, bool shouldRepeat = true)
    {
        if (MusicNode.Playing)
        {
            MusicNode.Stop();

            MusicNode.VolumeDb = 0;
        }

        MusicNode.Stream = PrepareStream(audioStream: audioStream, shouldRepeat: shouldRepeat);

        if (fadeInTime > 0.0f)
        {
            MusicNode.VolumeDb = -80;
            MusicNode.Play();
            Tween musicTween = GetTree().CreateTween();
            musicTween.TweenProperty(MusicNode, AudioStreamPlayerProperties.VolumeDb, 0, fadeInTime);
            await ToSignal(musicTween, Tween.SignalName.Finished);
        }

        MusicNode.Play();
    }

    public async void StopMusicTrack(float fadeOutTime = 0.0f)
    {
        if (MusicNode.Playing)
        {
            if (fadeOutTime > 0)
            {
                Tween musicTween = GetTree().CreateTween();
                musicTween.TweenProperty(MusicNode, AudioStreamPlayerProperties.VolumeDb, -80, fadeOutTime);

                await ToSignal(musicTween, Tween.SignalName.Finished);
            }

            MusicNode.Stop();
        }
    }

    public async void ResumeMusic(float fadeInTime = 0.0f)
    {
        if (!MusicNode.Playing)
        {
            if (fadeInTime > 0)
            {
                MusicNode.VolumeDb = -80;
                MusicNode.Play();
                Tween musicTween = GetTree().CreateTween();
                musicTween.TweenProperty(MusicNode, AudioStreamPlayerProperties.VolumeDb, 0, fadeInTime);
                await ToSignal(musicTween, Tween.SignalName.Finished);
            }
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

    public void PlayMusic(AudioStream audioStream, float fadeInTime = 0.0f, float previousTrackFadeOutTime = 0.0f, bool shouldRepeat = true)
    {
        StopMusicTrack(fadeOutTime: previousTrackFadeOutTime);
        StartMusicTrack(audioStream: audioStream, fadeInTime: fadeInTime);
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

    private static AudioStream PrepareStream(AudioStream audioStream, bool shouldRepeat)
    {
        if (audioStream == null)
        {
            GD.PushWarning("Audio stream cannot be played - is null");
            return null;
        }



        if (audioStream.Duplicate() is not AudioStream streamCopy)
        {
            GD.PushWarning("Audio stream parameter is not an usable AudioStream");
            return null;
        }


        if (shouldRepeat)
        {
            switch (streamCopy)
            {
                case AudioStreamOggVorbis ogg:
                    ogg.Loop = shouldRepeat;
                    break;

                case AudioStreamMP3 mp3:
                    mp3.Loop = shouldRepeat;
                    break;

                case AudioStreamWav wav:
                    wav.LoopMode = shouldRepeat ? AudioStreamWav.LoopModeEnum.Forward : AudioStreamWav.LoopModeEnum.Disabled;
                    break;
            }
        }

        return streamCopy;
    }
}
