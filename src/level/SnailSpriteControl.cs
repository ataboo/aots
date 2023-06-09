using Godot;
using System;
using System.Linq;

public class SnailSpriteControl : Node2D
{
    private int debugCount;

    [Export]
    public NodePath snailSpritePath;
    private AnimatedSprite _snailSprite;

    private GameManager _gameManager;

    [Export]
    public NodePath[] hat1Paths;
    private Sprite[] _hat1Sprites;

    [Export]
    public NodePath[] hat2Paths;
    private Sprite[] _hat2Sprites;

    [Export]
    public NodePath[] hat3Paths;
    private Sprite[] _hat3Sprites;

    [Export]
    public NodePath[] hat4Paths;
    private Sprite[] _hat4Sprites;

    [Export]
    public NodePath[] hat5Paths;
    private Sprite[] _hat5Sprites;

    private Node2D _parent;

    [Export]
    public NodePath hat90BasePath;
    private Node2D _hat90Base;

    [Export]
    public bool showRoomMode = false;

    [Export]
    public NodePath hat45BasePath;
    private Node2D _hat45Base;

    private Vector2 _hat90BaseStartPos;
    private Vector2 _hat45BaseStartPos;

    private bool _blinkPending = false;

    [Export]
    public NodePath blinkTimerPath;
    private Timer _blinkTimer;

    [Export] public NodePath frontTentaclePath;
    private Node2D _frontTentacle;


    [Export] public NodePath backTentaclePath;
    private Node2D _backTentacle;

    public override void _Ready()
    {
        _snailSprite = GetNode<AnimatedSprite>(snailSpritePath) ?? throw new NullReferenceException();
        _gameManager = GetNode<GameManager>("/root/GameManager") ?? throw new NullReferenceException();
        _parent = GetParent<Node2D>();
        _hat90Base = GetNode<Node2D>(hat90BasePath) ?? throw new NullReferenceException();
        _hat45Base = GetNode<Node2D>(hat45BasePath) ?? throw new NullReferenceException();
        _blinkTimer = GetNode<Timer>(blinkTimerPath) ?? throw new NullReferenceException();
        _frontTentacle = GetNode<Node2D>(frontTentaclePath) ?? throw new NullReferenceException();
        _backTentacle = GetNode<Node2D>(backTentaclePath) ?? throw new NullReferenceException();

        _hat1Sprites = hat1Paths.Select(p => GetNode<Sprite>(p) ?? throw new NullReferenceException()).ToArray();
        _hat2Sprites = hat2Paths.Select(p => GetNode<Sprite>(p) ?? throw new NullReferenceException()).ToArray();
        _hat3Sprites = hat3Paths.Select(p => GetNode<Sprite>(p) ?? throw new NullReferenceException()).ToArray();
        _hat4Sprites = hat4Paths.Select(p => GetNode<Sprite>(p) ?? throw new NullReferenceException()).ToArray();
        _hat5Sprites = hat5Paths.Select(p => GetNode<Sprite>(p) ?? throw new NullReferenceException()).ToArray();

        _hat90BaseStartPos = _hat90Base.Position;
        _hat45BaseStartPos = _hat45Base.Position;

        _blinkTimer.Connect("timeout", this, nameof(OnBlinkTime));
        _blinkTimer.OneShot = true;

        if(showRoomMode) {
            _hat45Base.Visible = false;
        }

        RenderHats();

        ScheduleBlink();
    }

    public override void _PhysicsProcess(float delta)
    {
        if(showRoomMode) {
            if(_blinkPending) {
                _blinkPending = false;
                _snailSprite.Frame = 0;
                _snailSprite.Play("blink90");
            }
        } else {
            var roundedPos = new Vector2(Mathf.Round(_parent.Position.x), Mathf.Round(_parent.Position.y));
            Position = roundedPos - _parent.Position;
        }
    }

    public void UpdateSnailSprite(SnailState state) {
        var heading = 0f;
        var octodrant = 0;
        if(state.onFloor || state.onWall) {
            if(state.onWall) {
                var pointUp = state.lastDirection >= 0;
                if(state.input.y < 0) {
                    pointUp = true;
                }
                if(state.input.y > 0) {
                    pointUp = false;
                } 

                if(pointUp) {
                    octodrant = state.rightWallInRange ? 4 : 3;
                } else {
                    octodrant = state.rightWallInRange ? -4 : -3;
                }
            } else {
                var refDir = Mathf.Abs(state.lastDirection) < 4 ? Vector2.Up : Vector2.Down;
                if(state.input.x > 0) {
                    refDir = Vector2.Up;
                } else if(state.input.x < 0) {
                    refDir = Vector2.Down;
                }

                heading = ((Vector2)state.floorNormal).AngleTo(refDir);
                octodrant = (int)(8 * heading / Mathf.Pi);
            }
        } else if(!state.leftWallInRange && !state.rightWallInRange) {
            heading = state.velocity.AngleTo(Vector2.Right);
            octodrant = (int)(8 * heading / Mathf.Pi) % 8;

            if(state.floatInput) {
                var flipFloat = Mathf.Abs(octodrant) > 3;
                _snailSprite.Play("float90");
                _snailSprite.FlipH = flipFloat;
                _hat90Base.Position = new Vector2(flipFloat ? -_hat90BaseStartPos.x : _hat90BaseStartPos.x, _hat90BaseStartPos.y);
                _snailSprite.Rotation = 0;
                _hat90Base.Visible = true;
                _hat45Base.Visible = false;

                var floatTentScale = new Vector2(flipFloat ? -1 : 1, 1);
                _frontTentacle.Scale = floatTentScale;
                _backTentacle.Scale = floatTentScale;
                
                var tentRotation = flipFloat ? -Mathf.Pi / 6 : Mathf.Pi / 6;
                _frontTentacle.Rotation = tentRotation;
                _backTentacle.Rotation = tentRotation;
                return;
            }
        } else {
            octodrant = state.lastDirection;
        }

        state.lastDirection = octodrant;

        var flipH = false;
        var ninety = false;

        switch(octodrant) {
            case 0:
                _snailSprite.Rotation = 0;
                ninety = true;
                _frontTentacle.Scale = new Vector2(1, 1);
                break;
            case 8:
            case 7:
            case -7:
                _snailSprite.Rotation = 0;
                flipH = true;
                ninety = true;
                break;
            case 4:
                _snailSprite.Rotation = -Mathf.Pi / 2f;
                ninety = true;
                break;
            case -4:
                _snailSprite.Rotation = -Mathf.Pi / 2f;
                ninety = true;
                flipH = true;
                break;
            case 3:
                _snailSprite.Rotation = Mathf.Pi / 2f;
                ninety = true;
                flipH = true;
                break;

            case -3:
                _snailSprite.Rotation = Mathf.Pi / 2f;
                ninety = true;
                flipH = false;
                break;
            case 1:
            case 2:
                _snailSprite.Rotation = 0;
                break;
            case -5:
            case -6:
                _snailSprite.Rotation = -Mathf.Pi/2f;
                flipH = true;
                break;
            case -1:
            case -2:
                _snailSprite.Rotation = Mathf.Pi/2f;
                break;
            case 5:
            case 6:
                _snailSprite.Rotation = 0;
                flipH = true;
                break;
        }


        _snailSprite.FlipH = flipH;
        UpdateSnailAnimation(ninety, state);

        _hat90Base.Visible = ninety;
        _hat45Base.Visible = !ninety;

        var tentacleScale = new Vector2(flipH ? -1 : 1, 1);
        var tentacleRotation = _snailSprite.Rotation;

        if(ninety) {
            _hat90Base.Scale = new Vector2(flipH ? -1 : 1, 1);
            _hat90Base.Position = new Vector2(flipH ? -_hat90BaseStartPos.x : _hat90BaseStartPos.x, _hat90BaseStartPos.y);
        } else {
            _hat45Base.Position = new Vector2(_hat45BaseStartPos.x * (flipH ? -1 : 1), _hat45BaseStartPos.y);
            _hat45Base.Scale = new Vector2(flipH ? -1 : 1, 1);

            tentacleRotation = _snailSprite.Rotation + (flipH ? Mathf.Pi / 4 : -Mathf.Pi / 4);
        }

        // if(state.velocity.LengthSquared() > 5) {
        //     tentacleScale.x *= 1.3f;
        // }

        _frontTentacle.Scale = tentacleScale;
        _backTentacle.Scale = tentacleScale;
        _frontTentacle.Rotation = tentacleRotation;
        _backTentacle.Rotation = tentacleRotation;

    }

    private void UpdateSnailAnimation(bool ninety, SnailState state)  {
        var animation = AnimationForState(ninety, state);
        
        if(_snailSprite.Animation != animation) {
            _snailSprite.Frame = 0;
            _snailSprite.Play(animation);
        }
    }

    private string AnimationForState(bool ninety, SnailState state) {
        var blinkWasPending = _blinkPending;
        _blinkPending = false;
        
        if(state.floatInput && !state.leftWallInRange && !state.rightWallInRange && !state.onFloor) {
            return "float90";
        }
        
        if(state.input == Vector2.Zero || (!state.onFloor && !state.onWall)) {
            if(blinkWasPending || (_snailSprite.Animation.StartsWith("blink") && _snailSprite.Playing)) {
                return ninety ? "blink90" : "blink45";
            }

            return ninety ? "still90" : "still45";
        }

        return ninety ? "eat90" : "eat45";
    }

    private void HideAllHats() {
        var allHats = _hat1Sprites
            .Concat(_hat2Sprites)
            .Concat(_hat3Sprites)
            .Concat(_hat4Sprites)
            .Concat(_hat5Sprites);
        foreach(var s in allHats) {
            s.Visible = false;
        }
    }

    private void ScheduleBlink() {
        _blinkTimer.Start((float)GD.RandRange(3, 7));
    }

    private void OnBlinkTime() {
        _blinkPending = true;
        ScheduleBlink();
    }

    public void RenderHats() {
        HideAllHats();
        
        Sprite[] hatSprites;
        switch(_gameManager.GameState.equippedHat) {
            case -1:
                return;
            case 0:
                hatSprites = _hat1Sprites;
                break;
            case 1:
                hatSprites = _hat2Sprites;
                break;
            case 2:
                hatSprites = _hat3Sprites;
                break;
            case 3:
                hatSprites = _hat4Sprites;
                break;
            case 4:
                hatSprites = _hat5Sprites;
                break;
            default:
                throw new NotSupportedException();

        }

        foreach(var s in hatSprites) {
            s.Visible = true;
        } 
    }
}
