using Godot;
using System;
using System.Linq;
using static Enums;

public class LevelControl : Node2D
{
	[Export]
	public NodePath snailPath;
	private SnailControl _snailControl;

	[Export]
	public NodePath hudPath;
	private HUDControl _hudControl;

	[Export]
	public NodePath victoryMenuPath;
	private VictoryMenuControl _victoryMenu;

	[Export] public NodePath heaterPath;
	private HeaterControl _heater;

	private Vector2 _startPosition;

	private LevelState _state;

	[Export]
	public int levelIndex = 0;

	private GameManager _gameManager; 

	[Export] NodePath soundPlayerPath;
	private AudioStreamPlayer _soundPlayer;

	[Export] AudioStream fishDieSound;

	[Export] NodePath[] bettaFishPath;
	private FishControl[] _bettaFish;

	[Export] NodePath shrimpPath;
	private FishControl _shrimp;

	[Export] NodePath blueFishPath;
	private FishControl _blueFish;

	public override void _Ready()
	{
		_snailControl = GetNode<SnailControl>(snailPath) ?? throw new NullReferenceException();
		_hudControl = GetNode<HUDControl>(hudPath) ?? throw new NullReferenceException();
		_victoryMenu = GetNode<VictoryMenuControl>(victoryMenuPath) ?? throw new NullReferenceException();
		_soundPlayer = GetNode<AudioStreamPlayer>(soundPlayerPath) ?? throw new NullReferenceException();
		_heater = GetNode<HeaterControl>(heaterPath) ?? throw new NullReferenceException();
		_bettaFish = bettaFishPath.Select(p => GetNode<FishControl>(p)).ToArray();
		_shrimp = GetNode<FishControl>(shrimpPath) ?? throw new NullReferenceException();
		_blueFish = GetNode<FishControl>(blueFishPath) ?? throw new NullReferenceException();
		_gameManager = GetNode<GameManager>("/root/GameManager") ?? throw new NullReferenceException();
		
		_startPosition = _snailControl.Position;

		if(_state == null) {
			InitStats();
		}
	}

	public override void _Process(float delta)
	{
		UpdateStats(delta);

		_hudControl.UpdateHUD(_snailControl.SnailState, _state);
	}

	public void OnShmooCountChanged(ShmooCount shmooCount) {
		if(_state == null) {
			InitStats();
		}

		_state.shmooCount = shmooCount;

		if(_state.WonGame()) {
			var hatUnlocked = false;
			var perfectLevelHat = VictoryHatUnlock();
			if(perfectLevelHat >= 0) {
				hatUnlocked = OnHatUnlocked(perfectLevelHat);
			}

			_victoryMenu.ShowVictory(_state, hatUnlocked);
		}
	}

	private int VictoryHatUnlock() {
		if(_state.fish1.health == 0 || _state.fish1.health == 1 || _state.fish1.health == 2) {
			return -1;
		}

		switch(levelIndex) {
			case 0:
				return 1;
			case 1:
				return 3;
			default:
				return -1;
		}
	}

	public void InitStats() {
		var startingStats = StatsHolder.StartingStats[levelIndex];
		_state = new LevelState() {
			levelIndex = levelIndex,
			fish1 = new FishState(){
				fishType = FishType.Bettas,
				health = startingStats.fish1Health,
				initialHealth = startingStats.fish1Health
			},
			fish2 = new FishState(){
				fishType = FishType.Shrimp,
				health = startingStats.fish2Health,
				initialHealth = startingStats.fish2Health
			},
			fish3 = new FishState(){
				fishType = FishType.Blue,
				health = startingStats.fish3Health,
				initialHealth = startingStats.fish3Health
			},
			shmooCount = new ShmooCount(),
			shmooDamageRate = startingStats.shmooDamageRate,
			tempDamageRate = startingStats.tempDamageRate,
			temperature = 0.5f,
		};

		_heater.SetLevelStats(startingStats);
	}

	private void UpdateStats(float delta) {
		var damage = 0f;
		if(_state.shmooCount.count > 0) {
			damage += delta * _state.shmooDamageRate;
		}

		_state.temperature = Mathf.Lerp(_state.temperature, _heater.DialLevel(), 0.1f * delta);
		_heater.UpdateGauge(_state.temperature);

		var tempImbalance = Mathf.Abs((_state.temperature - 0.5f) * 2);

		if(tempImbalance > 0.25f) {
			damage += delta * _state.tempDamageRate * tempImbalance;
		}


		var allFish = new FishState[]{_state.fish1, _state.fish2, _state.fish3};

		foreach(var fish in allFish) {
			if(fish.health == 0) {
				continue;
			}

			fish.health = Mathf.Max(0, fish.health - damage);

			if(fish.health == 0) {
				fish.justDied = true;
				_soundPlayer.Stream = fishDieSound;
				_soundPlayer.Play();
			}
		}

		if(_state.fish1.justDied) {
			foreach(var betta in _bettaFish) {
				betta.KillFish();
			}
		}

		if(_state.fish2.justDied) {
			_shrimp.KillFish();
		}

		if(_state.fish3.justDied) {
			_blueFish.KillFish();
		}

		if(_state.LostGame()) {
			_victoryMenu.ShowVictory(_state, false);
		}
	}

    public bool OnHatUnlocked(int hatId)
    {
		if(_gameManager.UnlockHat(hatId)) {
			_hudControl.ShowHatUnlock(hatId);
			return true;
		}

		return false;
    }
}
