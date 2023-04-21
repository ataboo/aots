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

    public static Color ColorForImbalance(this float value, float balancePoint = 0.5f) {
        value = Mathf.Clamp(value, 0, 1);
        balancePoint = Mathf.Clamp(balancePoint, 0, 1);

        var maxImbalance = Mathf.Max(balancePoint, 1 - balancePoint);

        var imbalance = Mathf.Abs((value - balancePoint)) / maxImbalance;

        return BalanceColor.LinearInterpolate(ImbalanceColor, imbalance);
    }
}