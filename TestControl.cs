using Godot;
using System;
using System.Threading.Tasks;

public partial class TestControl : Control
{
    [Export]
    public Button PlayMusicButton;

    [Export]
    public Button PauseMusicButton;

    [Export]
    public Button PlaySoundEffectButton;

    [Export]
    public Button PlayTransitionInButton;

    [Export]
    public Button PlayerTransitionOut;

    public override void _Ready()
    {
        base._Ready();

        AudioStream soundEffect = ResourceLoader.Load<AudioStream>($"{Paths.SoundEffects}/world_sfx.ogg");

        AudioStream music = ResourceLoader.Load<AudioStream>($"{Paths.Music}/default_music.mp3");

        PlaySoundEffectButton.Pressed += async () =>
        {
            await AudioManager.Instance.PlaySoundEffect(soundEffect: soundEffect);
        };

        PlayMusicButton.Pressed += () =>
        {
            AudioManager.Instance.PlayMusic(audioStream: music);
        };

        PauseMusicButton.Pressed += () =>
        {
            AudioManager.Instance.PauseMusic();
        };

        PlayTransitionInButton.Pressed += () =>
        {
            TransitionManager.Instance.PlayTransition(transitionType: TransitionManager.TransitionType.FadeOut);
        };

        PlayerTransitionOut.Pressed += () =>
        {
            TransitionManager.Instance.PlayTransition(transitionType: TransitionManager.TransitionType.FadeIn);
        };

    }

}
