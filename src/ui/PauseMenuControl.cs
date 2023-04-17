using Godot;
using System;

public class PauseMenuControl : Control
{
    private GameManager _gameManager;
    
    [Export]
    public NodePath levelControlPath;
    private LevelControl _levelControl;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager") ?? throw new NullReferenceException();
        _levelControl = GetNode<LevelControl>(levelControlPath) ?? throw new NullReferenceException();
    }

	public override void _UnhandledInput(InputEvent @event) {
		if(Input.IsActionJustPressed("pause")) {
			SetPaused(!GetTree().Paused);
		}
	}

    public void OnRestartClick() {
        SetPaused(false);
        _gameManager.LoadLevel(_levelControl.levelIndex);
    }

    public void OnExitClick() {
        SetPaused(false);
        _gameManager.LoadMainMenu();
    }

    public void OnResumeClick() {
        SetPaused(false);
    }

    private void SetPaused(bool paused) {
        Visible = paused;
        GetTree().Paused = paused;
    }
}
