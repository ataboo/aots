using Godot;
using System;
using System.Threading.Tasks;

public class RoarControl : TextureRect
{
    [Export] public NodePath spritePath;
    private AnimatedSprite _sprite;

    [Export] public NodePath audioPath;
    private AudioStreamPlayer _audio;

    [Export] public NodePath particlePath;
    private CPUParticles2D _particles;


    
    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite>(spritePath) ?? throw new NullReferenceException();
        _audio = GetNode<AudioStreamPlayer>(audioPath) ?? throw new NullReferenceException();
        _particles = GetNode<CPUParticles2D>(particlePath) ?? throw new NullReferenceException();
    }

    public async Task PlayRoar() {
        _sprite.Frame = 0;
        _sprite.Play();

        await ToSignal(GetTree().CreateTimer(.7f), "timeout");

        _audio.Play();

        _particles.Emitting = true;
        await ToSignal(GetTree().CreateTimer(3f), "timeout");
        _particles.Emitting = false;
    }
}
