using Godot;

public static class GodotExtensions {
    public static Color BalanceColor = new Color(0, 1, 0);
    public static Color ImbalanceColor = new Color(1, 0, 0);

    public static TElem RandomElement<TElem>(this RandomNumberGenerator rng, TElem[] col) {
        if(col.Length == 0) {
            return default;
        }
        
        var idx = rng.RandiRange(0, col.Length-1);
        return col[idx];
    }

    public static Color ColorForTemperature(this float temp) {
        temp = Mathf.Clamp(temp, 0, 1);

        var imbalance = Mathf.Abs((temp - 0.5f) * 2);

        return BalanceColor.LinearInterpolate(ImbalanceColor, imbalance);
    }
}