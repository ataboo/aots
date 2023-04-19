using Godot;

public class AutoTileId: Godot.Object {
    public string tileName;
    public int x;
    public int y;
    public int tileId = -1;

    public static AutoTileId FromTileId(int tileId, int x, int y) {
        return new AutoTileId() {
            tileId = tileId,
            x = x,
            y = y,
        };
    }

    public static AutoTileId FromName(TileSet tileSet, string tileName, int x, int y) {
        var id = AutoTileId.FromTileId(tileSet.FindTileByName(tileName), x, y);
        id.tileName = tileName;
        
        if(id.tileId < 0) {
            GD.PushError($"Failed to find tile with name: {tileName}");
        }
        
        return id; 
    }

    public override bool Equals(object obj)
    {
        return obj is AutoTileId id &&
                x == id.x &&
                y == id.y &&
                tileId == id.tileId;
    }

    public override int GetHashCode()
    {
        int hashCode = -1355968855;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        hashCode = hashCode * -1521134295 + tileId.GetHashCode();
        return hashCode;
    }
}