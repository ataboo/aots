using Godot;
using System;

public class HeaterControl : Node2D
{
    [Export] public NodePath dialPath;
    private DialControl _dialControl;

    [Export] public NodePath gaugePath;
    private ColorRect _gaugeRect;

    private float _initialGaugeHeight;

    public override void _Ready()
    {
        _gaugeRect = GetNode<ColorRect>(gaugePath) ?? throw new NullReferenceException();
        _dialControl = GetNode<DialControl>(dialPath) ?? throw new NullReferenceException();
        _initialGaugeHeight = _gaugeRect.RectSize.y;
    }

    public void UpdateGauge(float temp) {
        _gaugeRect.MarginTop = (1-temp) * _initialGaugeHeight;

        _gaugeRect.Color = temp.ColorForImbalance();
    }

    public float DialLevel() => _dialControl?.Level() ?? 0.5f;

    public void SetLevelStats(StartingLevelStats startingStats)
    {
        _dialControl.SetLevelStats(startingStats);
    }
}
