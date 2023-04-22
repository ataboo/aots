using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;

public class RoarControl : TextureRect
{
    [Export] public NodePath spritePath;
    private AnimatedSprite _sprite;

    [Export] public NodePath audioPath;
    private AudioStreamPlayer _audio;

    [Export] public NodePath particlePath;
    private CPUParticles2D _particles;

    [Export] NodePath[] hat1Paths;
    [Export] NodePath[] hat2Paths;
    [Export] NodePath[] hat3Paths;
    [Export] NodePath[] hat4Paths;
    [Export] NodePath[] hat5Paths;

    private Sprite[][] _hatSprites;

    private GameManager _gameManager;

    [Export] NodePath hatAnimatorPath;
    private AnimationPlayer _hatAnimation;


    
    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager") ?? throw new NullReferenceException();
        _sprite = GetNode<AnimatedSprite>(spritePath) ?? throw new NullReferenceException();
        _audio = GetNode<AudioStreamPlayer>(audioPath) ?? throw new NullReferenceException();
        _particles = GetNode<CPUParticles2D>(particlePath) ?? throw new NullReferenceException();
        _hatAnimation = GetNode<AnimationPlayer>(hatAnimatorPath)?? throw new NullReferenceException();

        _hatSprites = new Sprite[][]{
            hat1Paths.Select(p => GetNode<Sprite>(p)).ToArray(),
            hat2Paths.Select(p => GetNode<Sprite>(p)).ToArray(),
            hat3Paths.Select(p => GetNode<Sprite>(p)).ToArray(),
            hat4Paths.Select(p => GetNode<Sprite>(p)).ToArray(),
            hat5Paths.Select(p => GetNode<Sprite>(p)).ToArray(),
        };
    }

    public void UpdateHats() {
        for(int i=0; i<5; i++) {
            foreach(var sprite in _hatSprites[i]) {
                sprite.Visible = _gameManager.GameState.equippedHat == i;
            }
        }
    }

    public async Task PlayRoar() {
        _sprite.Frame = 0;
        _sprite.Play();
        _hatAnimation.Play("RoarHat");

        await ToSignal(GetTree().CreateTimer(.7f), "timeout");

        _audio.Play();

        _particles.Emitting = true;
        await ToSignal(GetTree().CreateTimer(3f), "timeout");
        _particles.Emitting = false;
    }
}
