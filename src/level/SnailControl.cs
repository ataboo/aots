using Godot;
using System;

public class SnailControl : KinematicBody2D
{
    private int _debugCount = 0;

    [Export]
    public NodePath rightWallDetectorPath;
    private Area2D _rightWallDetector;

    [Export]
    public NodePath leftWallDetectorPath;
    private Area2D _leftWallDetector;

    [Export]
    public NodePath snailSpritePath;
    private Sprite _snailSprite;

    private bool _onFloor;
    private bool _leftWallInRange = false;
    private bool _rightWallInRange = false;

    private bool _onWall;

    private Vector2? _floorNormal = null;

    [Export]
    public float speed = 1200f;

    [Export]
    public float jumpSpeed = 1800f;

    [Export]
    public float gravity = 4000f;

    [Export(PropertyHint.Range, "0.0,1.0")]
    public float friction = 0.1f;

    [Export(PropertyHint.Range, "0.0,1.0")]
    public float accel = 0.25f;

    [Export]
    public float wallStick = 1f;

    private Vector2 velocity = Vector2.Zero;


    private Vector2 _inputAxes;

    public override void _Ready()
    {
        _snailSprite = GetNode<Sprite>(snailSpritePath) ?? throw new NullReferenceException();
        _rightWallDetector = GetNode<Area2D>(rightWallDetectorPath) ?? throw new NullReferenceException();
        _leftWallDetector = GetNode<Area2D>(leftWallDetectorPath) ?? throw new NullReferenceException();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        
    }

    public override void _PhysicsProcess(float delta)
    {
        _inputAxes = Vector2.Zero;
        if(Input.IsActionPressed("move_right")) {
            _inputAxes += Vector2.Right;
        }
        if(Input.IsActionPressed("move_left")) {
            _inputAxes += Vector2.Left;
        }
        if(Input.IsActionPressed("move_up")) {
            _inputAxes += Vector2.Up;
        }
        if(Input.IsActionPressed("move_down")) {
            _inputAxes += Vector2.Down;
        }

        // var justJumped = Input.IsActionJustPressed("jump") && this._onFloor || this._onWall;

        if(_inputAxes.x != 0) {
            velocity.x = Mathf.Lerp(velocity.x, _inputAxes.x * speed, accel);
        } else {
            velocity.x = Mathf.Lerp(velocity.x, 0, friction);
        }

        if(!_onFloor && !_onWall) {
            velocity.y += gravity * delta;
        }

        if(_onWall) {
            if(_inputAxes.y != 0) {
                velocity.y = Mathf.Lerp(velocity.y, _inputAxes.y * speed, accel);
            } else {
                velocity.y = Mathf.Lerp(velocity.y, 0, friction);
            }

            if(_floorNormal != null) {
                if(((Vector2)_floorNormal).x != _inputAxes.x) {
                    velocity -= (Vector2)_floorNormal * wallStick;
                }
            }
        }

        velocity = MoveAndSlide(velocity, Vector2.Up, stopOnSlope: true);
        
        _floorNormal = GetLastSlideCollision()?.Normal;
        if(_floorNormal != null && ((Vector2)_floorNormal).y > 0) {
            _floorNormal = null;
        }
        _onFloor = _floorNormal != null && ((Vector2)_floorNormal).y < -0.2f;
        _onWall = _floorNormal != null && !_onFloor;

        _leftWallInRange = _leftWallDetector.GetOverlappingBodies().Count > 0;
        _rightWallInRange = _rightWallDetector.GetOverlappingBodies().Count > 0;

        UpdateSnailSprite(_inputAxes);
    }

    private void UpdateSnailSprite(Vector2 input) {

        var roundedPos = new Vector2(Mathf.Round(Position.x), Mathf.Round(Position.y));
        
        _snailSprite.Position = roundedPos - Position;

        if(_onFloor || _onWall) {
            if(_onWall) {
                if(input.y < 0) {
                    _snailSprite.FlipH = _leftWallInRange;
                } else if(input.y > 0) {
                    _snailSprite.FlipH = !_leftWallInRange;
                }
            } else {
                if(input.x > 0) {
                    _snailSprite.FlipH = false;
                } else if(input.x < 0) {
                    _snailSprite.FlipH = true;
                }
            }

            _snailSprite.Rotation = ((Vector2)_floorNormal).Angle() + Mathf.Pi / 2;
        } else {
            if(velocity.x > 0) {
                _snailSprite.FlipH = false;
                _snailSprite.Rotation = new Vector2(velocity.x, velocity.y).Angle();
            } else if(velocity.x < 0) {
                _snailSprite.FlipH = true;
                _snailSprite.Rotation = new Vector2(velocity.x, velocity.y).Angle() + Mathf.Pi;
            } else {
                // _snailSprite.Rotation = 0;
            }
        }


    }
}
