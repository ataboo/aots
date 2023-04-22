using Godot;
using System;

public class PushplantControl : Node2D
{
    [Export] public float breakDistance = 20f;
    private Vector2 _startingPos;

    [Export] public NodePath springJointPath;
    private DampedSpringJoint2D _springJoint;

    [Export] public NodePath plantRBPath;
    private RigidBody2D _plantRB;

    [Export] public int waterSurfaceHeight = -710;

    private bool _broken = false;

    public override void _Ready()
    {   
        _springJoint = GetNode<DampedSpringJoint2D>(springJointPath) ?? throw new NullReferenceException();
        _plantRB = GetNode<RigidBody2D>(plantRBPath) ?? throw new NullReferenceException();

        _plantRB.LinearDamp = 0.8f;
        _plantRB.AngularDamp = 0.8f;
        _startingPos = _plantRB.Position;
    }

    public override void _PhysicsProcess(float delta)
    {
        if(!_broken && (_plantRB.Position - _startingPos).Length() > breakDistance) {
            _springJoint.NodeA = new NodePath();
            _springJoint.NodeB = new NodePath();

            // _plantRB.Mode = RigidBody2D.ModeEnum.Kinematic;
            _broken = true;
        }

        if(_broken) {
            _plantRB.GravityScale = _plantRB.Position.y < waterSurfaceHeight ? 0.3f : -0.1f;
            // Position = _plantRB.GravityScale = 0
        }
    }
}
