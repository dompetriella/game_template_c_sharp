using Godot;
using System;
using System.Threading.Tasks;

public partial class TestControl : Control
{
    private Button PlayMusicButton;

    public override void _Ready()
    {
        base._Ready();

        Button PlaySoundEffectButton = GetNode<Button>("%PlaySoundEffectButton");
        Button PlayMusicButton = GetNode<Button>("%PlayMusicButton");

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

    }

}
