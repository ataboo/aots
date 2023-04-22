using Godot;
using System;

public class TentacleSegmentControl : RigidBody2D
{
    [Export] public NodePath nextSegmentPath;
    private TentacleSegmentControl _nextSegment;

    public Vector2 TailPosition => Position + (Vector2.Left * 3).Rotated(Rotation);

    public override void _Ready()
    {
        if(nextSegmentPath != null) {
            _nextSegment = GetNode<TentacleSegmentControl>(nextSegmentPath);
        }
    }
}
