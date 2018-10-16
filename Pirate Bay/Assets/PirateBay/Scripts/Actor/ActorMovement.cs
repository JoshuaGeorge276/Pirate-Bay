using System;
using System.Collections;
using System.Collections.Generic;
using Core;
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

    [SerializeField] private float _nextMoveInputThreshold = 0.01f;

    private bool _isMoving = false;
    public bool IsMoving
    {
        get { return _isMoving; }
    }
    private Vector2 _lastPos;
    private Vector2 _target;
    private Vector2 _facingDir;

    public Vector2 CurrentMoveDir
    {
        get { return _target - _lastPos; }
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
	}

    public override void ManagedUpdate(float a_fDeltaTime)
    {
        if (!_isMoving)
        {
            Vector2 moveDir;
            if (!ShouldMove(out moveDir))
                return;

            StartMove(moveDir);
            ResetInput();
        }

        Move(a_fDeltaTime);
    }

    private bool ShouldMove(out Vector2 a_MoveDir)
    {
        Vector2 moveDir = GetMoveDir();

        if (moveDir == Vector2.zero)
        {
            a_MoveDir = Vector2.zero;
            return false;
        }

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
        _facingDir = a_moveDir;
        _target = _lastPos + a_moveDir;
        _worldObject.GridPos = (WorldGridPos)_target;
        _isMoving = true;
    }

    private void Move(float a_deltaTime)
    {
        Vector2 delta = _target - LocalPosition;
        float absLength = Mathf.Abs(delta.magnitude);
        if (absLength > 0)
        {
            Vector2 nextPos = Vector2.MoveTowards(LocalPosition, _target, _moveSpeed * a_deltaTime);

            LocalPosition = nextPos;


            if (absLength < _nextMoveInputThreshold && ShouldMove(out nextPos))
            {
                _lastPos = _target;
                StartMove(nextPos);
            }
            return;
        }

        _lastPos = _target;
        _isMoving = false;
    }

    private void ResetInput()
    {
        _input.x = 0f;
        _input.y = 0f;
    }
}
