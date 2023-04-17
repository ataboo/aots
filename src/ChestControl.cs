using System;
using Godot;

public class ChestControl : Area2D
{
    [Export]
    public int chestId;

    private LevelControl _levelControl;

    private bool _popped = false;

    public override void _Ready()
    {
        _levelControl = GetNode<LevelControl>("/root/Level") ?? throw new NullReferenceException();

        Connect("body_entered", this, nameof(OnChestEntered));
    }

    public void OnChestEntered(PhysicsBody2D other) {
        if(!_popped && other is SnailControl) {
            _popped = true;
            _levelControl.OnChestOpened(chestId);
        }
    }
}
