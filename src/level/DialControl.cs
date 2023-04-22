using System;
using Godot;

public class DialControl : KinematicBody2D
{
    private bool _topActive = false;
    private bool _bottomActive = false;

    private float _velocity = 0;

    [Export] public float pushedAccel = 0.25f;

    [Export] public float friction = 0.25f;

    [Export] public float pushedSpeed = 100f;

    [Export] public float minY = -90f;

    [Export] public float maxY = 78f;

    private float _heightRange;

    private float _maxWanderCooldown = 0f;

    private float _wanderCooldown = 0f;

    private float _wanderRate = 0f;

    public override void _Ready()
    {
        _heightRange = maxY - minY;
    }

    public void SetLevelStats(StartingLevelStats startStats) {
        _wanderRate = startStats.tempWanderRate;
        _maxWanderCooldown = startStats.tempWanderCooldown;
        _wanderCooldown = _maxWanderCooldown;
    }

    public override void _PhysicsProcess(float delta)
    {
        float input = 0;
        if(_topActive) {
            input = pushedSpeed;
        } else if (_bottomActive) {
            input = -pushedSpeed;
        }

        if(_topActive || _bottomActive) {
            _wanderCooldown = _maxWanderCooldown;
        } else if((_wanderCooldown -= delta) < 0) {
            input = Level() > 0.5f ? -pushedSpeed * _wanderRate : pushedSpeed * _wanderRate;
        }

        if(input != 0) {
            _velocity = Mathf.Lerp(_velocity, input, pushedAccel);
        } else {
            _velocity = Mathf.Lerp(_velocity, 0, friction);
        }

        Position = new Vector2(Position.x, Mathf.Clamp(Position.y + _velocity * delta, minY, maxY));
    }

    public void OnBodyEntered(PhysicsBody2D other, bool top) {
        if(other is SnailControl) {
            if(top) {
                _topActive = true;
            } else {
                _bottomActive = true;
            }
        }
    }

    public void OnBodyExited(PhysicsBody2D other, bool top) {
        if(other is SnailControl) {
            if(top) {
                _topActive = false;
            } else {
                _bottomActive = false;
            }
        }
    }

    public float Level() {
        return 1 - (Position.y - minY) / _heightRange; 
    }
}
