using Godot;
using System;

public class SnailSpriteControl : Node2D
{
    private int debugCount;

    [Export]
    public NodePath snailControlPath;
    private SnailControl _snailControl;

    [Export]
    public NodePath snail90Path;
    private Sprite _snail90Sprite;

    [Export]
    public NodePath snail45Path;
    private Sprite _snail45Sprite;

    public override void _Ready()
    {
        _snailControl = GetNode<SnailControl>(snailControlPath) ?? throw new NullReferenceException();
        _snail90Sprite = GetNode<Sprite>(snail90Path) ?? throw new NullReferenceException();
        _snail45Sprite = GetNode<Sprite>(snail45Path) ?? throw new NullReferenceException();
    }

    public override void _PhysicsProcess(float delta)
    {
        var roundedPos = new Vector2(Mathf.Round(_snailControl.Position.x), Mathf.Round(_snailControl.Position.y));
        Position = roundedPos - _snailControl.Position;
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
        } else {
            octodrant = state.lastDirection;
        }

        state.lastDirection = octodrant;


        var flipH = false;
        
        switch(octodrant) {
            case 0:
                _snail90Sprite.Visible = true;
                _snail45Sprite.Visible = false;
                _snail90Sprite.Rotation = 0;
                break;
            case 8:
                _snail90Sprite.Visible = true;
                _snail45Sprite.Visible = false;
                _snail90Sprite.Rotation = 0;
                flipH = true;
                break;
            case 7:
            case -7:
                _snail90Sprite.Visible = true;
                _snail45Sprite.Visible = false;
                _snail90Sprite.Rotation = 0;
                flipH = true;
                break;
            case 4:
                _snail90Sprite.Visible = true;
                _snail45Sprite.Visible = false;
                _snail90Sprite.Rotation = -Mathf.Pi / 2f;
                break;
            case -4:
                _snail90Sprite.Visible = true;
                _snail45Sprite.Visible = false;
                _snail90Sprite.Rotation = -Mathf.Pi / 2f;
                flipH = true;
                break;
            case 3:
                _snail90Sprite.Visible = true;
                _snail45Sprite.Visible = false;
                _snail90Sprite.Rotation = Mathf.Pi / 2f;
                flipH = true;
                break;

            case -3:
                _snail90Sprite.Visible = true;
                _snail45Sprite.Visible = false;
                _snail90Sprite.Rotation = Mathf.Pi / 2f;
                flipH = false;
                break;
            case 1:
            case 2:
                _snail90Sprite.Visible = false;
                _snail45Sprite.Visible = true;
                _snail45Sprite.Rotation = 0;
                break;
            case -5:
            case -6:
                _snail90Sprite.Visible = false;
                _snail45Sprite.Visible = true;
                _snail45Sprite.Rotation = -Mathf.Pi/2f;
                flipH = true;
                break;
            case -1:
            case -2:
                _snail90Sprite.Visible = false;
                _snail45Sprite.Visible = true;
                _snail45Sprite.Rotation = Mathf.Pi/2;
                break;
            case 5:
            case 6:
                _snail90Sprite.Visible = false;
                _snail45Sprite.Visible = true;
                _snail45Sprite.Rotation = 0;
                flipH = true;
                break;
        }

        _snail45Sprite.FlipH = flipH;
        _snail90Sprite.FlipH = flipH;
    }
}
