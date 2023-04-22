using Godot;
using System;

public class LevelPickControl : Panel
{
    [Export] public NodePath level1BtnPath;
    private Button _level1Button;

    [Export] public NodePath level2BtnPath;
    private Button _level2Button;
    
    private GameManager _gameManager;
    
    public override void _Ready()
    {
        _level1Button = GetNode<Button>(level1BtnPath) ?? throw new NullReferenceException();
        _level2Button = GetNode<Button>(level2BtnPath) ?? throw new NullReferenceException();
        _gameManager = GetNode<GameManager>("/root/GameManager") ?? throw new NullReferenceException();

        if(!_gameManager.GameState.level2Unlocked) {
            _level2Button.Disabled = true;
        }
    }

    public void OnLevelButtonClick(int levelIdx) {
        _gameManager.LoadLevel(levelIdx);
    }
}
