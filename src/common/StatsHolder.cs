public static class StatsHolder {
    public static StartingLevelStats[] StartingStats = new StartingLevelStats[]{
        new StartingLevelStats() {
            fish1Health = 50,
            fish2Health = 70,
            fish3Health = 100,
            shmooDamageRate = 0.2f,
            tempDamageRate = 2f,
            tempWanderCooldown = 15f,
            tempWanderRate = 0.01f,
        },
        new StartingLevelStats() {
            fish1Health = 50,
            fish2Health = 70,
            fish3Health = 100,
            shmooDamageRate = 0.2f,
            tempDamageRate = 2f,
            tempWanderCooldown = 10f,
            tempWanderRate = 0.02f,
        }
    };
}

public class StartingLevelStats {
    public float fish1Health;
    public float fish2Health;
    public float fish3Health;
    public float shmooDamageRate;
    public float tempDamageRate;
    public float tempWanderCooldown;
    public float tempWanderRate;
}