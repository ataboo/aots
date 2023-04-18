using Godot;
using System;

public class BubblerControl : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }


    public void OnBodyEntered(PhysicsBody2D other) {
        if(other is SnailControl snail) {
            snail.InBubbles = true;
        }
    }


    public void OnBodyExited(PhysicsBody2D other) {
        if(other is SnailControl snail) {
            snail.InBubbles = false;
        }
    }
}
