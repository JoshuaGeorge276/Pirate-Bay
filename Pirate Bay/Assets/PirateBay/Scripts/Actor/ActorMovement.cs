using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.EventSystem;
using PirateBay.Events.Camera;
using UnityEngine;
using PirateBay.World;

[RequireComponent(typeof(WorldObject))]
public class ActorMovement : ManagedBehaviour
{
    private Vector2 _input;
    public Vector2 Input { set { _input = value; } }

    private Vector2 Position { get { return transform.position; } set { transform.position = value; } }
    private Vector2 LocalPosition { get {return transform.localPosition;} set {transform.localPosition = value;} }
    private Transform _transform;

    private WorldObject _worldObject;

    [SerializeField] private float _moveSpeed = 3.5f;

    private bool _isMoving = false;
    public bool IsMoving
    {
        get { return _isMoving; }
    }
    private Vector2 _lastPos;
    private Vector2 _target;
    private Vector2 _facingDir;

    private Vector2 _currentMoveDir;
    public Vector2 CurrentMoveDir
    {
        get { return _currentMoveDir; }
    }

    public Vector2 GridPosition
    {
        get { return _lastPos; }
    }

    public Vector2 Forward
    {
        get { return _facingDir; }
    }

    protected override void Awake()
    {
        base.Awake();

        _transform = transform;
        _lastPos = LocalPosition;
        _target = LocalPosition;
        _facingDir = Vector2.down;

        _worldObject = GetComponent<WorldObject>();
    }

    // Use this for initialization
	protected override void Start () 
	{
		base.Start();
        EventSystem.Instance.Raise(new CamEvents.SetCamTargetEvent(transform));
	}

    public override void ManagedUpdate(float a_fDeltaTime)
    {
        Move(a_fDeltaTime);

        if (!_isMoving)
        {
            Debug.Log("Not Moving");
            Vector2 moveDir;
            if (!ShouldMove(out moveDir))
            {
                _currentMoveDir = Vector2.zero;
                return;
            }
                
            Debug.Log("Recieved new movedir");
            StartMove(moveDir);
            //ResetInput();
        }
    }

    private bool ShouldMove(out Vector2 a_MoveDir)
    {
        Vector2 moveDir = GetMoveDir();
        a_MoveDir = Vector2.zero;
        if (moveDir == Vector2.zero)
            return false;

        // Check in world bounds 
        Vector2 newPos = LocalPosition + moveDir;

        if (newPos.x < 0 || newPos.x > World.Instance.Bounds.x)
            return false;
        if (newPos.y < 0 || newPos.y > World.Instance.Bounds.y)
            return false;

        a_MoveDir = moveDir;
        return true;
    }

    private bool switchLastDir = false;
    private Vector2 GetMoveDir()
    {
        float absX = Mathf.Abs(_input.x);
        float absY = Mathf.Abs(_input.y);

        if (absX <= float.Epsilon && absY <= float.Epsilon)
            return Vector3.zero;

        Vector2 inputNormalised = _input.normalized;
        int rndX = Mathf.RoundToInt(inputNormalised.x);
        int rndY = Mathf.RoundToInt(inputNormalised.y);

        if (absX > absY)
        {
            return Vector2.right * rndX;
        }
        else if(absY > absX)
        {
            return Vector2.up * rndY;
        }

        if (switchLastDir)
        {
            switchLastDir = !switchLastDir;
            return Vector2.right * rndX;
        }
        else
        {
            switchLastDir = !switchLastDir;
            return Vector2.up * rndY;
        }
    }

    private void StartMove(Vector2 a_moveDir)
    {
        // TODO Collision Check
        _currentMoveDir = a_moveDir;
        _facingDir = a_moveDir;
        _target = _lastPos + a_moveDir;
        _worldObject.GridPos = (WorldGridPos)_target;
        _isMoving = true;
    }

    private void Move(float a_deltaTime)
    {
        if (SqrDistToTarget() > 0) // Not yet reached target
        {
            Vector2 nextPos = Vector2.MoveTowards(LocalPosition, _target, _moveSpeed * a_deltaTime);
            LocalPosition = nextPos;

            // Recheck if target is reached
            if(SqrDistToTarget() > 0)
                return;
        }

        _lastPos = _target;
        _isMoving = false;
    }

    private float SqrDistToTarget()
    {
        Vector2 delta = _target - LocalPosition;
        return Mathf.Abs(delta.sqrMagnitude);
    }

    private void ResetInput()
    {
        _input.x = 0f;
        _input.y = 0f;
    }
}
