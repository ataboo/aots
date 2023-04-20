using Godot;
using System;

[Tool]
public class HealthBarControl : Control
{
    [Export]
    public NodePath barPath;
    private ColorRect _colorBar;

    [Export]
    public NodePath iconPath;
    private TextureRect _icon;

    private float _startWidth;

    [Export]
    public Color barColor;

    [Export]
    public Texture iconTexture;


    public override void _Ready()
    {
        _colorBar = GetNode<ColorRect>(barPath) ?? throw new NullReferenceException();
        _icon = GetNode<TextureRect>(iconPath) ?? throw new NullReferenceException();

        _startWidth = _colorBar.RectSize.x;
        _colorBar.Color = barColor;

        _icon.Texture = iconTexture;
    }

    public void SetLevel(float t, Color? barColor = null) {
        t = Mathf.Clamp(t, 0, 1);
        _colorBar.RectSize = new Vector2(_startWidth * t, _colorBar.RectSize.y);

        if(barColor != null) {
            _colorBar.Color = (Color)barColor;
        }
    }

    public void SetIcon(Texture texture) {
        _icon.Texture = texture;
    }
}
