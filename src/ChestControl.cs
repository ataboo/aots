using System;
using Godot;

public class ChestControl : Area2D
{
    [Export]
    public int hatId;

    private LevelControl _levelControl;

    private bool _popped = false;

    [Export] public NodePath chestSpritePath;
    private AnimatedSprite _chestSprite;

    [Export] public NodePath particlesPath;
    private CPUParticles2D _particles;

    [Export] public NodePath audioPath;
    private AudioStreamPlayer2D _audioSource;

    

    public override void _Ready()
    {
        _levelControl = GetNode<LevelControl>("/root/Level") ?? throw new NullReferenceException();
        _chestSprite = GetNode<AnimatedSprite>(chestSpritePath) ?? throw new NullReferenceException();
        _particles = GetNode<CPUParticles2D>(particlesPath) ?? throw new NullReferenceException();
        _audioSource = GetNode<AudioStreamPlayer2D>(audioPath) ?? throw new NullReferenceException();

        Connect("body_entered", this, nameof(OnChestEntered));
    }

    public void OnChestEntered(PhysicsBody2D other) {
        if(!_popped && other is SnailControl) {
            _popped = true;
            _levelControl.OnHatUnlocked(hatId);

            _chestSprite.Play("open");
            _particles.Emitting = true;
            _audioSource.Play();
        }
    }
}
