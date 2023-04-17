using Godot;
using System.Collections.Generic;

[Tool]
public class TileSetControl : TileSet
{
    private string[] _spikeTileNames = new string[]{
        "spike_up",
        "spike_down",
        "spike_left",
        "spike_right",
    };

    private Dictionary<AutoTileId, Vector2[]> _shmooPoints = null;

    private HashSet<int> _deathTileIds;

    public bool TileIsSpike(int tileId) {
        if(_deathTileIds == null) {
            InitDeathTileIds();
        }

        return _deathTileIds.Contains(tileId);
    }

    private void InitDeathTileIds() {
        _deathTileIds = new HashSet<int>();
        foreach(var name in _spikeTileNames) {
            var tileId = FindTileByName(name);
            if(tileId < 0) {
                GD.PushError($"Failed to find tile with name {name}");
            }

            _deathTileIds.Add(tileId);
        }
    }

    public Vector2[] LocalShmooPoints(int tileId, Vector2 autotilePos) {
        if(_shmooPoints == null || _shmooPoints.Values.Count == 0) {
            InitShmooPoints();
        }

        if(_shmooPoints.TryGetValue(new AutoTileId(tileId, (int)autotilePos.x, (int)autotilePos.y), out var points)) {
            GD.Print($"Id: {tileId}, x {autotilePos.y}, y {autotilePos.x}");
            return points;
        }

        return new Vector2[]{};
    }

    private void InitShmooPoints() {
        GD.Print("init?");
        _shmooPoints = new Dictionary<AutoTileId, Vector2[]>{
            {AutoTileId.FromName(this, "sand_flat", 0, 0), new Vector2[]{new Vector2(0, 0)}},
            {AutoTileId.FromName(this, "sand_corner", 0, 0), new Vector2[]{new Vector2(0, 0)}},
        };
    }
}


