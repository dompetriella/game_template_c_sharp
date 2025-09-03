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
    public Button NextPageButton;

    public override void _Ready()
    {
        base._Ready();

        AudioStream soundEffect = ResourceLoader.Load<AudioStream>($"{Paths.SoundEffects}/world_sfx.ogg");

        AudioStream music = ResourceLoader.Load<AudioStream>($"{Paths.Music}/default_music.mp3");

        PackedScene nextPage = ResourceLoader.Load<PackedScene>("uid://dnxdcwwed7h5p");

        PlaySoundEffectButton.Pressed += async () =>
        {
            await AudioManager.Instance.PlaySoundEffect(soundEffect: soundEffect);
        };

        PlayMusicButton.Pressed += () =>
        {
            AudioManager.Instance.StartMusicTrack(audioStream: music);
        };

        PauseMusicButton.Pressed += () =>
        {
            AudioManager.Instance.PauseMusic();
        };

        NextPageButton.Pressed += () =>
        {
            ScaffoldManager.Instance.ScaffoldNewSceneTree(newSceneTree: nextPage.Instantiate());
        };
    }
}
