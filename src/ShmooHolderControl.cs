using Godot;
using System;

[Tool]
public class ShmooHolderControl : Node2D
{
	[Signal]
	public delegate ShmooCount ShmooCountChanged();

	private static RandomNumberGenerator _rng = new RandomNumberGenerator();

	[Export]
	public NodePath mainTileMapPath;
	private TileMapControl _mainTileMap;

	[Export]
	public PackedScene[] _shmooPrefabs;

	private ShmooCount _shmooCount;


	private bool _generateShmoo = false;
	[Export]
	public bool GenerateShmoo {
		get {
			return _generateShmoo;
		}
		set {
			if(Engine.EditorHint && value) {
				RunShmooGeneration();
			}
			_generateShmoo = value;
		}
	}

	private bool _clearShmoo = false;
	[Export]
	public bool ClearShmoo {
		get {
			return _clearShmoo;
		}
		set {
			if(Engine.EditorHint && value) {
				RunClearShmoo();
			}
			_clearShmoo = value;
		}
	}

	public override void _Ready() {
		var childCount = GetChildCount();
		_shmooCount = new ShmooCount() {
			count = childCount,
			initial = childCount
		};
		
		if(!Engine.EditorHint) {
			CallDeferred(nameof(EmitShmooCount));
		}
	}

	private void RunShmooGeneration() {
		if(_mainTileMap == null) {
			_mainTileMap = GetNode<TileMapControl>(mainTileMapPath) ?? throw new NullReferenceException();
		}

		var shmooSpawnPoints = _mainTileMap.GetShmooSpawnPoints();

		foreach(var shmooPoint in shmooSpawnPoints) {
			var prefab = _rng.RandomElement(_shmooPrefabs);

			var instance = prefab.Instance<Node2D>();
			instance.Position = shmooPoint;
			AddChild(instance, true);
			instance.Owner = GetTree().EditedSceneRoot;
		}
	}

	private void RunClearShmoo() {
		if(_mainTileMap == null) {
			_mainTileMap = GetNode<TileMapControl>(mainTileMapPath) ?? throw new NullReferenceException();
		}

		for(var i=GetChildCount()-1; i>=0; i--) {
			GetChild(i).QueueFree();
		}
		return;
	}

	public void OnShmooDied() {
		_shmooCount.count--;
		EmitShmooCount();
	}

	private void EmitShmooCount() {
		EmitSignal(nameof(ShmooCountChanged), _shmooCount);
	}
}
