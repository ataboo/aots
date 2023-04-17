using Godot;
using System;

public class HealthBarControl : Control
{
    [Export]
    public NodePath coloredPanelPath;
    private Panel _coloredPanel;

    [Export]
    public NodePath iconPath;
    private TextureRect _icon;

    private float _startWidth;

    [Export]
    public StyleBoxFlat barStyle;

    [Export]
    public Texture iconTexture;


    public override void _Ready()
    {
        _coloredPanel = GetNode<Panel>(coloredPanelPath) ?? throw new NullReferenceException();
        _icon = GetNode<TextureRect>(iconPath) ?? throw new NullReferenceException();

        _startWidth = _coloredPanel.RectSize.x;

        _icon.Texture = iconTexture;

        // _coloredPanel.RemoveStyleboxOverride("panel");
        _coloredPanel.AddStyleboxOverride("panel", barStyle);
    }

    public void SetLevel(float t) {
        t = Mathf.Clamp(t, 0, 1);
        _coloredPanel.RectSize = new Vector2(_startWidth * t, _coloredPanel.RectSize.y);
    }

    public void SetIcon(Texture texture) {
        _icon.Texture = texture;
    }
}
