using Godot;
using System.Linq;

[Tool]
public class TileMapControl : TileMap
{
    private const float jitterMax = 1f;

    private TileSetControl _tileSetControl => TileSet as TileSetControl;

    private static RandomNumberGenerator _rng = new RandomNumberGenerator();

    public override void _Ready()
    {
    }

    public bool PositionIsSpike(Vector2 position, Vector2 colNormal) {
        var halfCell = position - (colNormal *32);
        var mapPos = WorldToMap(halfCell);
        var tileId = GetCellv(mapPos);
        if(tileId < 0) {
            return false;
        }

        return _tileSetControl.TileIsSpike(tileId);
    }

    public Vector2[] GetShmooSpawnPoints() {
        _rng.Randomize();
        var shmooPoints = new Vector2[]{};

        foreach(Vector2 mapPos in GetUsedCells()) {
            var cellId = GetCell((int)mapPos.x, (int)mapPos.y);
            var autotilePos = GetCellAutotileCoord((int)mapPos.x, (int)mapPos.y);

            var aboveCellId = GetCell((int)mapPos.x, (int)mapPos.y-1);
            if(_tileSetControl.TileIsSpike(aboveCellId)) {
                continue;
            }

            var localPoints = _tileSetControl.LocalShmooPoints(cellId, autotilePos);

            shmooPoints = shmooPoints.Concat(localPoints.Select(lp => {
                var jitter = new Vector2(_rng.RandfRange(-jitterMax, jitterMax), _rng.RandfRange(-jitterMax, jitterMax));
                return MapToWorld(mapPos) + lp + jitter;
            })).ToArray();
        }
        
        return shmooPoints.ToArray();
    }
}
