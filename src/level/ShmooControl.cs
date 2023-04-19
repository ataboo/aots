using Godot;
using System;

public class ShmooControl : Area2D
{
    private ShmooHolderControl _shmooHolder;
    private bool _dead = false;

    private AudioStreamPlayer _audioPlayer;

    [Export]
    public AudioStream[] popStreams;

    public override void _Ready()
    {
        _shmooHolder = GetNode<ShmooHolderControl>("..") ?? throw new NullReferenceException();
        _audioPlayer = GetNode<AudioStreamPlayer>("EffectPlayer") ?? throw new NullReferenceException();

        Connect("body_entered", this, nameof(OnShmooBodyEntered));

        _audioPlayer.Stream = (new RandomNumberGenerator()).RandomElement(popStreams);

    }

    public void OnShmooBodyEntered(PhysicsBody2D other) {
        if(!_dead && other is SnailControl) {
            _dead = true;
            Visible = false;
            _shmooHolder.OnShmooDied();
            _audioPlayer.Play();

            GetTree().CreateTimer(2).Connect("timeout", this, nameof(OnDeathTimerOver));    
        }
    }

    public void OnDeathTimerOver() {
        QueueFree();
    }
}
