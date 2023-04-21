using Godot;
using System;
using static Enums;

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

    [Export] Texture bettasLg;
    [Export] Texture bettasDeadLg;

    [Export] Texture shrimpLg;
    [Export] Texture shrimpDeadLg;

    [Export] Texture blueLg;
    [Export] Texture blueDeadLg;

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

        _heaterBar.SetLevel(levelState.temperature, levelState.temperature.ColorForImbalance(0.5f));
    }
    
    private void UpdateFishHealth(HealthBarControl healthBar, FishState fishState) {
        healthBar.SetLevel(fishState.HealthProgress, fishState.HealthProgress.ColorForImbalance(1.0f));
        if(fishState.justDied) {
            healthBar.SetIcon(SmallTextureForFish(fishState));
        }
    }

    public Texture LargeTextureForFish(FishState fish) {
        switch(fish.fishType) {
            case FishType.Bettas:
                return fish.health > 0 ? bettasLg : bettasDeadLg;
            case FishType.Shrimp:
                return fish.health > 0 ? shrimpLg : shrimpDeadLg;
            case FishType.Blue:
                return fish.health > 0 ? blueLg : blueDeadLg;
            default:
                throw new NotImplementedException();
        }
    }

    public Texture SmallTextureForFish(FishState fish) {
        switch(fish.fishType) {
            case FishType.Bettas:
                return fish.health > 0 ? liveFishTexture : deadFishTexture;
            case FishType.Shrimp:
                return fish.health > 0 ? liveFishTexture : deadFishTexture;
            case FishType.Blue:
                return fish.health > 0 ? liveFishTexture : deadFishTexture;
            default:
                throw new NotImplementedException();
        }
    }

    public void ShowHatUnlock(int hatId)
    {
        _hatUnlock.OnHatUnlocked(hatId);
    }
}
