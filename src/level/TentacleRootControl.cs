using Godot;
using System;
using System.Collections.Generic;

public class TentacleRootControl : Node2D
{
    [Export] public Color tentacleColour = new Color("f9c89b");

    [Export] public NodePath rootSegmentPath;
    private Node2D _rootSegment;

    private TentacleSegmentControl[] _segments;

    public override void _Ready()
    {
        _rootSegment = GetNode<Node2D>(rootSegmentPath) ?? throw new NullReferenceException();
        var segmentList = new List<TentacleSegmentControl>();
        foreach(var child in GetChildren()) {
            if(child is TentacleSegmentControl tsc) {
                segmentList.Add(tsc);
            }
        } 
        _segments = segmentList.ToArray();
    }

    public override void _PhysicsProcess(float delta)
    {
        Update();
    }

    public override void _Draw()
    {
        if(_segments == null) {
            return;
        }

        DrawLine(_rootSegment.Position, _segments[0].Position, tentacleColour);
        for(int i=0; i<_segments.Length-1; i++) {
            var first = _segments[i];
            var next = _segments[i+1];

            DrawLine(first.Position, next.Position, tentacleColour);
        }
    }
}
