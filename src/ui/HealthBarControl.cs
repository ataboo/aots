using Godot;
using System;

public class HealthBarControl : Control
{
    [Export]
    public NodePath coloredPanelPath;
    private Panel _coloredPanel;

    [Export]
    public NodePath labelPath;
    private Label _label;

    private float _startWidth;

    private StyleBoxFlat _panelStyle;


    public override void _Ready()
    {
        _coloredPanel = GetNode<Panel>(coloredPanelPath) ?? throw new NullReferenceException();
        _label = GetNode<Label>(labelPath) ?? throw new NullReferenceException();

        _startWidth = _coloredPanel.RectSize.x;

    }

    public void InitializeHealthBar(string label, StyleBox styleBox = null) {
        if(styleBox != null) {
            this.AddStyleboxOverride("Panel", styleBox);
        }
        this._label.Text = label;
    }

    public void SetLevel(float t) {
        t = Mathf.Clamp(t, 0, 1);
        _coloredPanel.RectSize = new Vector2(_startWidth * t, _coloredPanel.RectSize.y);
    }
}
