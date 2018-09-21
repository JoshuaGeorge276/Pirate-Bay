using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class ActorMovement : ManagedBehaviour
{
    private Vector2 _input;
    public Vector2 Input { set { _input = value; } }

    private Vector2 _velocity;

    [SerializeField]
    private float _acceleration;
    [SerializeField]
    private float _runMultiplier;
    [SerializeField]
    private float _maxWalkSpeed;
    [SerializeField]
    private float _maxRunSpeed;

    [SerializeField]
    private float _drag;

    private Vector2 Position { get { return transform.position; } set { transform.position = value; } }

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
	}

    public override void ManagedUpdate(float a_fDeltaTime)
    {
        Vector2 curinput = _input;

        Vector2 additiveVelocity = curinput * _acceleration * Time.deltaTime;

        // TODO Drag
        if(additiveVelocity.sqrMagnitude <= 0) // No new Movement
        {
            // Apply Drag
            Vector2 negVelocity = -_velocity.normalized;

            if(negVelocity.sqrMagnitude > 0.1f)
                additiveVelocity =  negVelocity * _drag  * Time.deltaTime;
        }

        _velocity += additiveVelocity;

        // TODO Max Speed Clamping
        _velocity = Vector2.ClampMagnitude(_velocity, _maxWalkSpeed);

        Vector2 newPos = Position;

        newPos += _velocity;

        Position = newPos;

        ResetInput();
    }

    private void ResetInput()
    {
        _input.x = 0f;
        _input.y = 0f;
    }
}
