using Godot;
using System;
using System.Linq;

public class FishControl : Sprite
{
    [Export] public Texture deadFish;

    [Export] NodePath[] waypointPaths;
    private Position2D[] _waypoints;

    [Export] float waypointDeviation = 20f;

    private Vector2 _velocity = Vector2.Zero;

    private Vector2? _waypoint = null;

    private Vector2 WayPoint => (Vector2)_waypoint;
    
    [Export] float accel = .25f;

    [Export] float friction = .1f;

    [Export] float speed = 140f;

    [Export] float closeEnough = 20f;

    private bool _dead;

    float coolDown = 0f;

    [Export] float surfaceHeight = -710;

    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        _rng.Randomize();
        _waypoints = waypointPaths.Select(p => GetNode<Position2D>(p)).ToArray();
    }

    public override void _PhysicsProcess(float delta)
    {
        if(!_dead) {
            LiveUpdate(delta);
        } else {
            DeadUpdate(delta);
        }


        Position += _velocity * delta;
        FlipH = _velocity.x < 0;
}

    private void LiveUpdate(float delta) {
        if(coolDown > 0) {
            coolDown -= delta;

            _velocity = _velocity.LinearInterpolate(Vector2.Zero, friction);

            return;
        }

        if(_waypoint == null) {
            _waypoint = PickWaypoint();
        }

        var deltaWay = WayPoint - Position;
        var rangeWay = deltaWay.Length();
        if(rangeWay < closeEnough) {
            coolDown = 2f;
            _waypoint = null;
            return;
        }

        var bearing = deltaWay / rangeWay;
        _velocity = _velocity.LinearInterpolate(bearing * speed, accel);
    }

    private void DeadUpdate(float delta) {
        if(Position.y > surfaceHeight) {
            _velocity = Vector2.Up * 20f;
        } else {
            _velocity = Vector2.Zero;
        }
    }

    public void KillFish() {
        _dead = true;
        Texture = deadFish;
    }

    private Vector2 PickWaypoint() {
        var pos = _rng.RandomElement(_waypoints).Position;
        return new Vector2(pos.x + _rng.RandfRange(-waypointDeviation, waypointDeviation), pos.y + _rng.RandfRange(-waypointDeviation, waypointDeviation));
    }
}
