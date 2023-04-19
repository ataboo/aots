using Godot;
using System;
using System.Threading.Tasks;

public class VictoryMenuControl : Panel
{
    [Export]
    public NodePath titleLabelPath;
    [Export]
    public NodePath fish1TexturePath;
    [Export]
    public NodePath fish2TexturePath;
    [Export]
    public NodePath fish3TexturePath;
    [Export]
    public NodePath hudControlPath;
    [Export]
    public NodePath nextButtonPath;

    [Export]
    public NodePath victoryPlayerPath;

    private AudioStreamPlayer _victoryPlayer;

    [Export] public AudioStream liveSound;

    [Export] public AudioStream deadSound;

    [Export] public AudioStream winSound;

    [Export] public AudioStream loseSound;

    private Label _titleLabel;
    private TextureRect _fish1Texture;
    private TextureRect _fish2Texture;
    private TextureRect _fish3Texture;

    private HUDControl _hudControl;
    private GameManager _gameManager;

    private LevelState _levelState;
    private Button _nextButton;

    public override void _Ready()
    {
        _titleLabel = GetNode<Label>(titleLabelPath) ?? throw new NullReferenceException();
        _fish1Texture = GetNode<TextureRect>(fish1TexturePath) ?? throw new NullReferenceException();
        _fish2Texture = GetNode<TextureRect>(fish2TexturePath) ?? throw new NullReferenceException();
        _fish3Texture = GetNode<TextureRect>(fish3TexturePath) ?? throw new NullReferenceException();
        _hudControl = GetNode<HUDControl>(hudControlPath) ?? throw new NullReferenceException();
        _nextButton = GetNode<Button>(nextButtonPath) ?? throw new NullReferenceException();
        _victoryPlayer = GetNode<AudioStreamPlayer>(victoryPlayerPath) ?? throw new NullReferenceException();
        _gameManager = GetNode<GameManager>("/root/GameManager");
    }

    async public Task ShowVictory(LevelState state) {
        _fish1Texture.Texture = null;
        _fish2Texture.Texture = null;
        _fish3Texture.Texture = null;
        _titleLabel.Text = "Ended";
        _nextButton.Visible = false;
        _levelState = state;

        GetTree().Paused = true;
        Visible = true;

        ShowFishResult(_fish1Texture, state.fish1);

        await ToSignal(GetTree().CreateTimer(.7f), "timeout");
        ShowFishResult(_fish2Texture, state.fish2);

        await ToSignal(GetTree().CreateTimer(.7f), "timeout");
        ShowFishResult(_fish3Texture, state.fish3);

        await ToSignal(GetTree().CreateTimer(.7f), "timeout");
        
        _titleLabel.Text = !state.WonGame() ? "Fail!" : "Victory!";
        _nextButton.Text = state.WonGame() ? "Next" : "Restart";
        _nextButton.Visible = true;

        _victoryPlayer.Stream = state.WonGame() ? winSound : loseSound;
        _victoryPlayer.Play();
    }

    private void ShowFishResult(TextureRect rect, FishState fishState) {
       rect.Texture = _hudControl.TextureForFish(fishState);
       _victoryPlayer.Stream = fishState.health > 0 ? liveSound : deadSound;
       _victoryPlayer.Play();

    }

    public void OnNextClick() {
        GetTree().Paused = false;

        if(_levelState.WonGame()) {
            var nextLevelIdx = _levelState.levelIndex + 1;
            if (nextLevelIdx >= StatsHolder.StartingStats.Length) {
                _gameManager.LoadMainMenu();
                // _gameManager.LoadEnd();
                return;
            } else {
                _gameManager.LoadLevel(nextLevelIdx);
            }
        } else {
            _gameManager.LoadLevel(_levelState.levelIndex);
        }
    }
}
