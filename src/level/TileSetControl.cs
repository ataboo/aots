using Godot;
using System.Collections.Generic;

public class TileSetControl : TileSet
{
    private string[] _spikeTileNames = new string[]{
        "spike_up",
        "spike_down",
        "spike_left",
        "spike_right",
    };

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
}
