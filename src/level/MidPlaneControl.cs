using Godot;
using System;

public class MidPlaneControl : Node2D
{
    [Export] public NodePath cameraPath;
    private Camera2D _camera;

    [Export] float scale = 0.5f;
     
    [Export] Vector2 offset = Vector2.Zero;

    [Export] int tileWidth = 1;

    private Sprite _childSprite;

    public override void _Ready()
    {
        _camera = GetNode<Camera2D>(cameraPath) ?? throw new NullReferenceException(); 
        _childSprite = GetChild<Sprite>(0);

        for(int i=0; i<tileWidth; i++) {
            var leftTile = _childSprite.Duplicate() as Sprite;
            leftTile.Position = new Vector2(-_childSprite.GetRect().Size.x * (i+1), 0);
            AddChild(leftTile);
            var rightTile = _childSprite.Duplicate() as Sprite;
            rightTile.Position = new Vector2(_childSprite.GetRect().Size.x * (i+1), 0); 
            AddChild(rightTile);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        Position = ((_camera.Position + offset) * scale).Round();
    }
}
