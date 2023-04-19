using Godot;
using System;

public class MenuMusicControl : AudioStreamPlayer
{
    [Export]
    public NodePath audioButtonPath;
    private TextureButton _audioButton;

    [Export]
    public NodePath musicButtonPath;
    private TextureButton _musicButton;

    [Export]
    public Texture audioOn;
    [Export]
    public Texture audioOff;
    [Export]
    public Texture musicOn;
    [Export]
    public Texture musicOff;

    private bool MasterBusMuted => AudioServer.IsBusMute(0);
    private bool MusicBusMuted => AudioServer.IsBusMute(1);

    public override void _Ready()
    {
        _audioButton = GetNode<TextureButton>(audioButtonPath) ?? throw new NullReferenceException();
        _musicButton = GetNode<TextureButton>(musicButtonPath) ?? throw new NullReferenceException();

        _audioButton.Connect("pressed", this, nameof(OnAudioClicked));
        _musicButton.Connect("pressed", this, nameof(OnMusicClicked));

        UpdateButtons();
    }

    public void OnAudioClicked() {
        AudioServer.SetBusMute(0, !MasterBusMuted);

        UpdateButtons();
    }

    public void OnMusicClicked() {
        if(MasterBusMuted) {
            return;
        }

        AudioServer.SetBusMute(1, !MusicBusMuted);

        UpdateButtons();
    }

    private void UpdateButtons() {
        _musicButton.TextureNormal = MasterBusMuted || MusicBusMuted ? musicOff : musicOn;
        _audioButton.TextureNormal = MasterBusMuted ? audioOff : audioOn;
    }
}
