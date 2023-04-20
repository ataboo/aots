public static class StatsHolder {
    public static StartingLevelStats[] StartingStats = new StartingLevelStats[]{
        new StartingLevelStats() {
            fish1Health = 50,
            fish2Health = 70,
            fish3Health = 100,
            shmooDamageRate = 0.5f,
            tempDamageRate = 1f,
        }
    };
}

public class StartingLevelStats {
    public float fish1Health;
    public float fish2Health;
    public float fish3Health;
    public float shmooDamageRate;
    public float tempDamageRate;
}