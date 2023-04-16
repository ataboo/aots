using Godot;
using System;

public class HUDControl : Control
{
    [Export]
    public NodePath barParentPath;
    private Control _barParent;

    [Export]
    public StyleBoxFlat healthBarStyle;

    private HealthBarControl _floatBar;

    public override void _Ready()
    {
        this._barParent = GetNode<Control>(barParentPath) ?? throw new NullReferenceException();

        _floatBar = _barParent.GetChild<HealthBarControl>(0);
    }

    public void AddHealthBars(string[] labels) {
        throw new NotImplementedException();
    }

    public void UpdateHUD(SnailState state) {
        this._floatBar.SetLevel(state.floatTime / state.maxFloatTime);
    }
}
