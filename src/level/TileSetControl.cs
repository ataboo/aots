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

        if(_shmooPoints.TryGetValue(AutoTileId.FromTileId(tileId, (int)autotilePos.x, (int)autotilePos.y), out var points)) {
            return points;
        }

        return new Vector2[]{};
    }

    private void InitShmooPoints() {
        _shmooPoints = new Dictionary<AutoTileId, Vector2[]>{
            {AutoTileId.FromName(this, "sand_corner", 0, 0), PointsAlongLine(new Vector2(8, 0), new Vector2(56, 0), 5)},
            {AutoTileId.FromName(this, "sand_corner", 1, 0), PointsAlongLine(new Vector2(8, 0), new Vector2(56, 0), 5)},

            {AutoTileId.FromName(this, "sand_pad", 0, 0), PointsAlongLine(new Vector2(16, 0), new Vector2(56, 0), 4)},
            {AutoTileId.FromName(this, "sand_pad", 1, 0), PointsAlongLine(new Vector2(8, 0), new Vector2(56, 0), 5)},
            {AutoTileId.FromName(this, "sand_pad", 2, 0), PointsAlongLine(new Vector2(8, 0), new Vector2(48, 0), 4)},

            {AutoTileId.FromName(this, "sand_flat", 0, 0), PointsAlongLine(new Vector2(8, 56), new Vector2(56, 0), 6)},
            {AutoTileId.FromName(this, "sand_flat", 1, 0), PointsAlongLine(new Vector2(8, 0), new Vector2(56, 0), 5)},
            {AutoTileId.FromName(this, "sand_flat", 2, 0), PointsAlongLine(new Vector2(8, 0), new Vector2(56, 56), 6)},
        };
    }

    private Vector2[] PointsAlongLine(Vector2 p1, Vector2 p2, int pointCount) {
        var deltaP = p2 - p1;
        var len = deltaP.Length();

        var points = new Vector2[pointCount];
        for(int i=0; i<pointCount; i++) {
            var t = (float)i / (pointCount-1);
            points[i] = p1 + (deltaP * t);
        }

        return points;
    }
}


