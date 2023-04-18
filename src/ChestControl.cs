using System;
using Godot;

public class ChestControl : Area2D
{
    [Export]
    public int chestId;

    private LevelControl _levelControl;

    private bool _popped = false;

    [Export] public NodePath chestSpritePath;
    private AnimatedSprite _chestSprite;

    [Export] public NodePath particlesPath;
    private CPUParticles2D _particles;

    

    public override void _Ready()
    {
        _levelControl = GetNode<LevelControl>("/root/Level") ?? throw new NullReferenceException();
        _chestSprite = GetNode<AnimatedSprite>(chestSpritePath) ?? throw new NullReferenceException();
        _particles = GetNode<CPUParticles2D>(particlesPath) ?? throw new NullReferenceException();

        Connect("body_entered", this, nameof(OnChestEntered));
    }

    public void OnChestEntered(PhysicsBody2D other) {
        if(!_popped && other is SnailControl) {
            _popped = true;
            _levelControl.OnChestOpened(chestId);

            _chestSprite.Play("open");
            _particles.Emitting = true;
        }
    }
}
