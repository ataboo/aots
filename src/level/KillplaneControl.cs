using Godot;

public class KillplaneControl : Area2D
{
    void OnBodyEntered(PhysicsBody2D other) {
        if(other is SnailControl sc) {
            sc.KillSnail();
        }
    }
}
