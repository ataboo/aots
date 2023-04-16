using Godot;

public class SnailState {
    public bool onFloor;
    public bool onWall;
    public bool leftWallInRange;
    public bool rightWallInRange;
    public bool floating;
    public Vector2 velocity;
    public Vector2 input;
    public bool floatInput;
    public Vector2? floorNormal;
    public float floatTime;
    public float maxFloatTime;
    public int lastDirection;
}