public static class StatsHolder {
    public static StartingLevelStats[] StartingStats = new StartingLevelStats[]{
        new StartingLevelStats() {
            fish1Health = 100,
            fish2Health = 120,
            fish3Health = 150,
            shmooDamageRate = 1f,
            cheatCode = "snail"
        }
    };
}

public class StartingLevelStats {
    public float fish1Health;
    public float fish2Health;
    public float fish3Health;
    public float shmooDamageRate;
    public string cheatCode;
}