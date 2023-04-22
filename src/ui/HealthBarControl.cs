using Godot;
using System;
using System.Threading.Tasks;

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

    private float _lastLevel = -1f;

    private const float BLINK_AMOUNT = 0.4f;

    public override void _Ready()
    {
        _colorBar = GetNode<ColorRect>(barPath) ?? throw new NullReferenceException();
        _icon = GetNode<TextureRect>(iconPath) ?? throw new NullReferenceException();

        _startWidth = _colorBar.RectSize.x;
        _colorBar.Color = barColor;

        _icon.Texture = iconTexture;
    }

    public async Task SetLevel(float t, Color barColor, bool blinkOnChange = false, bool blinkAtInterval = false) {
        t = Mathf.Clamp(t, 0, 1);
        _colorBar.RectSize = new Vector2(_startWidth * t, _colorBar.RectSize.y);

        if(blinkOnChange) {
            if(_lastLevel != t) {
                if(_colorBar.Color == barColor) {
                    _colorBar.Color = new Color(Mathf.Clamp(barColor.r + BLINK_AMOUNT, 0, 1), Mathf.Clamp(barColor.g + BLINK_AMOUNT, 0, 1), Mathf.Clamp(barColor.b + BLINK_AMOUNT, 0, 1));
                }
                await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
                _colorBar.Color = barColor;

                _lastLevel = t;

                _colorBar.Color = barColor;
            }
        } else if(blinkAtInterval) {
            if((Godot.Time.GetTicksMsec() % 1000) > 500) {
                _colorBar.Color = new Color(Mathf.Clamp(barColor.r + BLINK_AMOUNT, 0, 1), Mathf.Clamp(barColor.g + BLINK_AMOUNT, 0, 1), Mathf.Clamp(barColor.b + BLINK_AMOUNT, 0, 1));
            } else {
                _colorBar.Color = barColor;
            }
        } else {
            _colorBar.Color = barColor;
        }
    }

    public void SetIcon(Texture texture) {
        _icon.Texture = texture;
    }
}
