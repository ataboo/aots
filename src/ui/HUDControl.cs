using Godot;
using System;

public class HUDControl : Control
{
    [Export]
    public NodePath barParentPath;
    private Control _barParent;

    [Export]
    public NodePath floatBarPath;
    [Export]
    public NodePath fish1BarPath;
    [Export]
    public NodePath fish2BarPath;
    [Export]
    public NodePath fish3BarPath;
    [Export]
    public NodePath shmooBarPath;
    [Export] public NodePath heaterBarPath;

    [Export]
    public Texture deadFishTexture;

    [Export]
    public Texture liveFishTexture;

    [Export]
    public NodePath hatUnlockPath;
    private HatUnlockControl _hatUnlock;

    private HealthBarControl _floatBar;
    private HealthBarControl _fish1Bar;
    private HealthBarControl _fish2Bar;
    private HealthBarControl _fish3Bar;
    private HealthBarControl _shmooBar;
    private HealthBarControl _heaterBar;

    private bool heaterInit = false;

    public override void _Ready()
    {
        this._barParent = GetNode<Control>(barParentPath) ?? throw new NullReferenceException();

        _floatBar = GetNode<HealthBarControl>(floatBarPath) ?? throw new NullReferenceException();
        _fish1Bar = GetNode<HealthBarControl>(fish1BarPath) ?? throw new NullReferenceException();
        _fish2Bar = GetNode<HealthBarControl>(fish2BarPath) ?? throw new NullReferenceException();
        _fish3Bar = GetNode<HealthBarControl>(fish3BarPath) ?? throw new NullReferenceException();
        _shmooBar = GetNode<HealthBarControl>(shmooBarPath) ?? throw new NullReferenceException();
        _heaterBar = GetNode<HealthBarControl>(heaterBarPath) ?? throw new NullReferenceException();
        _hatUnlock = GetNode<HatUnlockControl>(hatUnlockPath) ?? throw new NullReferenceException();
    }

    public void UpdateHUD(SnailState snailState, LevelState levelState) {
        this._floatBar.SetLevel(snailState.floatTime / snailState.maxFloatTime);
        
        UpdateFishHealth(_fish1Bar, levelState.fish1);
        UpdateFishHealth(_fish2Bar, levelState.fish2);
        UpdateFishHealth(_fish3Bar, levelState.fish3);

        this._shmooBar.SetLevel(1 - levelState.shmooCount.CountProgress);

        if(!_heaterBar.Visible) {
            _heaterBar.Visible = true;       
        }

        _heaterBar.SetLevel(levelState.temperature, levelState.temperature.ColorForTemperature());
    }
    
    private void UpdateFishHealth(HealthBarControl healthBar, FishState fishState) {
        healthBar.SetLevel(fishState.HealthProgress);
        if(fishState.health == 0) {
            healthBar.SetIcon(TextureForFish(fishState));
        }
    }

    public Texture TextureForFish(FishState fish) {
        if(fish.health == 0) {
            return deadFishTexture;
        } else {
            return liveFishTexture;
        }
    }

    public void ShowHatUnlock(int hatId)
    {
        _hatUnlock.OnHatUnlocked(hatId);
    }
}
