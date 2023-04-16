using Godot;
using System;

public class CameraControl : Camera2D
{
    [Export]
    public NodePath targetPath;
    private Node2D _target;

    public override void _Ready()
    {
        _target = GetNode<Node2D>(targetPath) ?? throw new NullReferenceException();
        Position = _target.Position;
    }

    public override void _PhysicsProcess(float delta)
    {
        var endPos = this.Position.LinearInterpolate(_target.Position, 0.8f * delta);
        endPos.x = Mathf.Round(endPos.x);
        endPos.y = Mathf.Round(endPos.y);
        this.Position = endPos;
    }
}
