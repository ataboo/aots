using System;
using Godot;

public class MainMenuControl : Control
{
    [Export]
    public NodePath instructionsPath;
    private Control _instructions;

    [Export]
    public NodePath creditsPath;
    private Control _credits;

    private GameManager _gameManager;

    [Export]
    public NodePath equipmentPath;
    private Control _equipment;

    [Export] NodePath levelPickPath;
    private Control _levelPick;

    public override void _Ready() {
        _instructions = GetNode<Control>(instructionsPath) ?? throw new NullReferenceException();
        _credits = GetNode<Control>(creditsPath) ?? throw new NullReferenceException();
        _equipment = GetNode<Control>(equipmentPath) ?? throw new NullReferenceException();
        _levelPick = GetNode<Control>(levelPickPath) ?? throw new NullReferenceException();
		_gameManager = GetNode<GameManager>("/root/GameManager") ?? throw new NullReferenceException();
    }

    public void OnInstructionsClick() {
        _instructions.Visible = true;
    }

    public void OnCloseInstructionsClick() {
        _instructions.Visible = false;
    }

    public void OnStartGameClick() {
        _levelPick.Visible = true;
    }

    public void OnCreditsClick() {
        _credits.Visible = true;
    }

    public void OnCreditsCloseClick() {
        _credits.Visible = false;
    }

    public void OnEquipmentClick() {
        _equipment.Visible = true;
    }

    public void OnEquipmentCloseClick() {
        _equipment.Visible = false;

    }

    public void OnLevelCloseClick() {
        _levelPick.Visible = false;
    }
}
