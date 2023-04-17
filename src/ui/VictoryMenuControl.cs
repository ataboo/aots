using Godot;
using System;

public class VictoryMenuControl : Panel
{
    [Export]
    public NodePath titleLabelPath;
    [Export]
    public NodePath cheatLabelPath;
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

    private Label _titleLabel;
    private Label _cheatCodeLabel;
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
        _cheatCodeLabel = GetNode<Label>(cheatLabelPath) ?? throw new NullReferenceException();
        _fish1Texture = GetNode<TextureRect>(fish1TexturePath) ?? throw new NullReferenceException();
        _fish2Texture = GetNode<TextureRect>(fish2TexturePath) ?? throw new NullReferenceException();
        _fish3Texture = GetNode<TextureRect>(fish3TexturePath) ?? throw new NullReferenceException();
        _hudControl = GetNode<HUDControl>(hudControlPath) ?? throw new NullReferenceException();
        _nextButton = GetNode<Button>(nextButtonPath) ?? throw new NullReferenceException();
        _gameManager = GetNode<GameManager>("/root/GameManager");
    }

    public void ShowVictory(LevelState state) {
        GetTree().Paused = true;
        Visible = true;

        _levelState = state;
        _titleLabel.Text = !state.WonGame() ? "Fail!" : "Victory!";
        _fish1Texture.Texture = _hudControl.TextureForFish(state.fish1);
        _fish2Texture.Texture = _hudControl.TextureForFish(state.fish2);
        _fish3Texture.Texture = _hudControl.TextureForFish(state.fish3);
        _nextButton.Text = state.WonGame() ? "Next" : "Restart";
        
        var cheatCode = StatsHolder.StartingStats[state.levelIndex].cheatCode;
        _cheatCodeLabel.Visible = cheatCode != "" && state.WonGame();
        _cheatCodeLabel.Text = $"Code: {cheatCode}";
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
