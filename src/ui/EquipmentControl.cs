using Godot;
using System;
using System.Linq;

public class EquipmentControl : Panel
{
    private GameManager _gameManager;

    [Export]
    public NodePath snailVisualPath;
    private SnailSpriteControl _snailVisual;

    [Export] 
    NodePath[] hatButtonPaths;
    private TextureButton[] _hatButtons;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager") ?? throw new NullReferenceException();
        _snailVisual = GetNode<SnailSpriteControl>(snailVisualPath) ?? throw new NullReferenceException();

        _hatButtons = hatButtonPaths.Select(p => GetNode<TextureButton>(p) ?? throw new NullReferenceException()).ToArray();

        for(int i=0; i<_hatButtons.Length; i++) {
            var button = _hatButtons[i];
            if(_gameManager.GameState.hatUnlocks[i]) {
                button.Disabled = false;
            } else {
                button.Disabled = true;
            }
        }
    }

    public void OnHatClick(int hatIdx) {
        _gameManager.SetEquippedHat(_gameManager.GameState.equippedHat == hatIdx ? -1 : hatIdx);

        _snailVisual.RenderHats();
    }
}
