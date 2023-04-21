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
    private SnailSpriteControl _snailSprite;

    [Export]
    public float speed = 120f;

    [Export]
    public float floatVelocity = 220f;

    [Export]
    public float floatAccel = 0.4f;

    [Export]
    public float gravity = 200f;

    [Export]
    public float bubbleForce = 300f;

    [Export(PropertyHint.Range, "0.0,1.0")]
    public float friction = 0.1f;

    [Export(PropertyHint.Range, "0.0,1.0")]
    public float accel = 0.25f;

    [Export]
    public float wallStick = 100;

    [Export]
    public float floatCapacity = 2.5f;

    [Export]
    public float floatRechargeFactor = 0.75f;

    [Export] public NodePath snailAudioPath;
    private AudioStreamPlayer _snailAudio;

    [Export] AudioStream dieSound;

    [Export] AudioStream inflateSound;

    private SnailState _state = new SnailState();

    public SnailState SnailState => _state;

    private Vector2 _startPosition;

    public bool InBubbles {get; set;}

    public override void _Ready()
    {
        _snailSprite = GetNode<SnailSpriteControl>(snailSpritePath) ?? throw new NullReferenceException();
        _rightWallDetector = GetNode<Area2D>(rightWallDetectorPath) ?? throw new NullReferenceException();
        _leftWallDetector = GetNode<Area2D>(leftWallDetectorPath) ?? throw new NullReferenceException();
        _snailAudio = GetNode<AudioStreamPlayer>(snailAudioPath) ?? throw new NullReferenceException();

        _startPosition = Position;

        InitState();
    }

    private void InitState() {
        _state = new SnailState() {
            floating = false,
            floatInput = false,
            floatTime = 0f,
            floorNormal = null,
            input = Vector2.Zero,
            lastDirection = 0,
            leftWallInRange = false,
            maxFloatTime = floatCapacity,
            onFloor = false,
            onWall = false,
            rightWallInRange = false,
            velocity = Vector2.Zero
        };
    }

    public override void _PhysicsProcess(float delta)
    {
        _state.input = Vector2.Zero;
        if(Input.IsActionPressed("move_right")) {
            _state.input += Vector2.Right;
        }
        if(Input.IsActionPressed("move_left")) {
            _state.input += Vector2.Left;
        }
        if(Input.IsActionPressed("move_up")) {
            _state.input += Vector2.Up;
        }
        if(Input.IsActionPressed("move_down")) {
            _state.input += Vector2.Down;
        }

        var jumping = Input.IsActionPressed("jump");
        var justJumped = !_state.floatInput && jumping;
        _state.floatInput = jumping;

        if(justJumped && _state.floatTime > 0) {
            _snailAudio.Stream = inflateSound;
            _snailAudio.Play();
        }

        if(_state.floatInput) {
            _state.floatTime -= delta;
        } else if(_state.onFloor || _state.onWall) {
            _state.floatTime += delta * floatRechargeFactor;
        }
        _state.floatTime = Mathf.Clamp(_state.floatTime, 0, floatCapacity);

        _state.floating = _state.floatInput && _state.floatTime > 0;

        if(_state.input.x != 0) {
            _state.velocity.x = Mathf.Lerp(_state.velocity.x, _state.input.x * speed, accel);
        } else {
            _state.velocity.x = Mathf.Lerp(_state.velocity.x, 0, friction);
        }

        if(_state.floating) {
            _state.velocity.y = Mathf.Lerp(_state.velocity.y, -floatVelocity, floatAccel);
        } else {
            if(_state.lastFloorNormal == null) {
                _state.velocity.y += gravity * delta;
            }

            if(_state.onWall) {
                if(_state.input.y != 0) {
                    _state.velocity.y = Mathf.Lerp(_state.velocity.y, _state.input.y * speed, accel);
                } else {
                    _state.velocity.y = Mathf.Lerp(_state.velocity.y, 0, friction);
                }
            }
            if(!InBubbles && _state.lastFloorNormal != null) {
                if(((Vector2)_state.lastFloorNormal).x != _state.input.x) {
                    _state.velocity -= (Vector2)_state.lastFloorNormal * wallStick;
                }
            }
        }

        if(InBubbles) {
            _state.velocity.y -= bubbleForce * delta;
        }
        
        _state.velocity = MoveAndSlide(_state.velocity, Vector2.Up, stopOnSlope: true);

        if(CollidedWithHazard()) {
            KillSnail();
        }
        
        _state.floorNormal = GetLastSlideCollision()?.Normal;
        if(_state.floorNormal != null && ((Vector2)_state.floorNormal).y > 0) {
            _state.floorNormal = null;
        }

        _state.onFloor = _state.floorNormal != null && ((Vector2)_state.floorNormal).y < -0.2f;
        _state.onWall = _state.floorNormal != null && !_state.onFloor;

        _state.leftWallInRange = _leftWallDetector.GetOverlappingBodies().Count > 0;
        _state.rightWallInRange = _rightWallDetector.GetOverlappingBodies().Count > 0;

        if(_state.floorNormal != null) {
            _state.lastFloorNormal = _state.floorNormal;
        }

        if(!_state.leftWallInRange && !_state.rightWallInRange) {
            _state.lastFloorNormal = null;
        }

        _snailSprite.UpdateSnailSprite(_state);
    }

    private bool CollidedWithHazard() {
        var col = GetLastSlideCollision();
        if(col?.Collider is TileMapControl tileMap) {
            return tileMap.PositionIsSpike(col.Position);
        }
        
        return false;
    }

    public void KillSnail() {
        _snailAudio.Stream = dieSound;
        _snailAudio.Play();
        RestartLevel();
    }

    private void RestartLevel() {
        Position = _startPosition;
        InitState();
    }
}
