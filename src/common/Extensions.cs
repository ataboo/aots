using Godot;

public static class GodotExtensions {
    public static TElem RandomElement<TElem>(this RandomNumberGenerator rng, TElem[] col) {
        if(col.Length == 0) {
            return default;
        }
        
        var idx = rng.RandiRange(0, col.Length-1);
        return col[idx];
    }
}