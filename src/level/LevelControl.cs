using Godot;
using System;

public class LevelControl : Node2D
{
    [Export]
    public NodePath snailPath;
    private SnailControl _snailControl;

    [Export]
    public NodePath hudPath;
    private HUDControl _hudControl;

    private Vector2 _startPosition;

    public override void _Ready()
    {
        _snailControl = GetNode<SnailControl>(snailPath) ?? throw new NullReferenceException();
        _hudControl = GetNode<HUDControl>(hudPath) ?? throw new NullReferenceException();

        _startPosition = _snailControl.Position;
    }

    public override void _Process(float delta)
    {
        _hudControl.UpdateHUD(_snailControl.SnailState);
    }


}
