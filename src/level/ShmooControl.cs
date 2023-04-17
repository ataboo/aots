using Godot;
using System;

public class ShmooControl : Area2D
{
    private ShmooHolderControl _shmooHolder;
    private bool _dead = false;

    public override void _Ready()
    {
        _shmooHolder = GetNode<ShmooHolderControl>("..") ?? throw new NullReferenceException();

        Connect("body_entered", this, nameof(OnShmooBodyEntered));
    }

    public void OnShmooBodyEntered(PhysicsBody2D other) {
        if(!_dead && other is SnailControl) {
            _dead = true;
            QueueFree();
            _shmooHolder.OnShmooDied();
        }
    }
}
