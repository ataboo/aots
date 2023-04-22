using System;
using Godot;

public class GameManager : Node2D
{
    [Export]
    public PackedScene[] levels;

    [Export]
    public PackedScene mainMenuScene;

    [Export]
    public PackedScene endScene;

    private GameState _gameState;

    public GameState GameState => _gameState;

    public override void _Ready()
    {
        _gameState = new GameState() {
            equippedHat = -1,
            hatUnlocks = new bool[]{
                false, // Pilgrim 
                false, // Fez
                false, // Horns
                false, // Beret
                false, // Cowboy
            },
            level2Unlocked = false,
        };
    }

    public void LoadLevel(int levelIdx) {
        if(levelIdx == 1) {
            _gameState.level2Unlocked = true;
        } 
        var nextScene = levels[levelIdx];
        GetTree().ChangeSceneTo(nextScene);
    }

    public void LoadEnd() {
        GetTree().ChangeSceneTo(mainMenuScene);
        GetNode<Control>("/root/MainMenu/Credits").Visible = true;
    }

    public void LoadMainMenu() {
        GetTree().ChangeSceneTo(mainMenuScene);
    }

    public void SetEquippedHat(int hatIdx)
    {
        if(hatIdx >= _gameState.hatUnlocks.Length) {
            throw new IndexOutOfRangeException($"Invalid hat index: {hatIdx}");
        }

        if(hatIdx < 0) {
            _gameState.equippedHat = -1;
            return;
        }

        if(_gameState.hatUnlocks[hatIdx]) {
            _gameState.equippedHat = hatIdx;
        }
    }

    public bool UnlockHat(int hatId) {
        if(_gameState.hatUnlocks[hatId]) {
            return false;
        }

        _gameState.hatUnlocks[hatId] = true;

        return true;
    }
}
