using Godot;
using static Enums;

public class LevelState: Godot.Object {
    public int levelIndex;

    public ShmooCount shmooCount;

    public FishState fish1;

    public FishState fish2;

    public FishState fish3;

    public float temperature;
    
    public float shmooDamageRate;

    public float tempDamageRate;

    public bool WonGame() {
        return shmooCount.count == 0;
    }

    public bool LostGame() {
        return fish1.health == 0 && fish2.health == 0 && fish3.health == 0;  
    }
}

public class ShmooCount: Godot.Object {
	public int count;
	public int initial;

    public float CountProgress => Mathf.Clamp((float)count / initial, 0, 1);
}

public class FishState {
    public FishType fishType;
    public float health;
    public float initialHealth;
    public float HealthProgress => Mathf.Clamp(health / initialHealth, 0, 1);
}