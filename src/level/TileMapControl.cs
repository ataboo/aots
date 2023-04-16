using Godot;
using System;

public class TileMapControl : TileMap
{
    private TileSetControl _tileSetControl;

    public override void _Ready()
    {
        this._tileSetControl = TileSet as TileSetControl ?? throw new NullReferenceException();
    }

    public bool PositionIsSpike(Vector2 position) {
        var tileId = GetCellv(WorldToMap(position));
        if(tileId < 0) {
            return false;
        }

        return _tileSetControl.TileIsSpike(tileId);
    }
}
