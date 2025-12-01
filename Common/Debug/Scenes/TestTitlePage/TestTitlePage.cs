using Godot;
using System;
using System.Threading.Tasks;

public partial class TestTitlePage : Control
{
    [Export]
    public Button PlayMusicButton;

    [Export]
    public Button PauseMusicButton;

    [Export]
    public Button PlaySoundEffectButton;

    [Export]
    public Button NextPageButton;

    [Export]
    public Button NotificationButton;

    [Export]
    public NavigationRouteComponent NavigationRouteComponent;

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
            AudioManager.Instance.StartMusicTrack(audioStream: music);
        };

        PauseMusicButton.Pressed += () =>
        {
            AudioManager.Instance.PauseMusic();
        };

        NextPageButton.Pressed += async () =>
        {
            ScaffoldManager.Instance.ScaffoldNewSceneTree(newSceneTree: NavigationRouteComponent.Scene.Instantiate());
            await NotificationManager.Instance.ShowNotification(messageText: "You changed pages!", showDurationInMs: 2000);
        };

        NotificationButton.Pressed += async () =>
        {
            await NotificationManager.Instance.ShowNotification(messageText: "This is [b]bold[/b] and [color=red]red[/color] text.");
        };
    }
}
