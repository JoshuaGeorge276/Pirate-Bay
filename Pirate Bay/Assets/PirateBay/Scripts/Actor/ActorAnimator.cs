using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class ActorAnimator : ManagedBehaviour
{
    [SerializeField]
    private ActorMovement _movement;

    [SerializeField]
    private Animator _animator;

    private Action updateDelegate = null;

    private int HorizontalHash;
    private int VerticalHash;

    protected override void Awake()
    {
        base.Awake();

        if(_movement == null)
            Debug.LogWarning("#ActorAnimator: Missing Movement Reference!");

        if(_animator == null)
            Debug.LogWarning("#ActorAnimator: Missing Animator Reference!");
    }

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();

	    if (_movement && _animator)
	    {
	        updateDelegate = UpdateAnimator;

	        HorizontalHash = Animator.StringToHash("Horizontal");
	        VerticalHash = Animator.StringToHash("Vertical");
	    }
	}

    public override void ManagedUpdate(float a_fDeltaTime)
    {
        if(updateDelegate != null)
            updateDelegate();
    }

    private void UpdateAnimator()
    {
        Vector2 curMoveDir = _movement.CurrentMoveDir;

        _animator.SetInteger(HorizontalHash, (int)curMoveDir.x);
        _animator.SetInteger(VerticalHash, (int) curMoveDir.y);
    }
}
