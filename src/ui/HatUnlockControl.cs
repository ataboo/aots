using Godot;
using System;
using System.Threading.Tasks;

public class HatUnlockControl : TextureRect
{
    [Export]
    public Texture[] hatTextures;

    [Export]
    public NodePath hatRectPath;
    private TextureRect _hatRect;

    public override void _Ready()
    {
        _hatRect = GetNode<TextureRect>(hatRectPath) ?? throw new NullReferenceException();
    }

    public async Task OnHatUnlocked(int hatId) {
        Visible = true;
        _hatRect.Texture = hatTextures[hatId];

        await ToSignal(GetTree().CreateTimer(3), "timeout");
        Visible = false;
    }
}
