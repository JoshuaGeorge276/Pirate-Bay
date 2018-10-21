using System.Collections;
using System.Collections.Generic;
using Core;
using Core.EventSystem;
using PirateBay.Events.Camera;
using UnityEngine;

public class TopDownCamController : ManagedBehaviour
{
    private Transform _myTransform;
    [SerializeField] private Transform _target;

    [SerializeField]
    private float _camSpeed = 3f;

    protected override void Awake()
    {
        base.Awake();
        _myTransform = transform;
    }

    protected override void Start()
    {
        EventSystem.Instance.Subscribe<CamEvents.SetCamTargetEvent>(SetTargetEventHandler);
    }

    private void SetTargetEventHandler(CamEvents.SetCamTargetEvent a_args)
    {
        SetTarget(a_args.target);
    }

    public void SetTarget(Transform a_target)
    {

        _target = a_target;
    }

    public override void ManagedUpdate(float a_fDeltaTime)
    {
        Vector3 myPos = _myTransform.position;
        Vector3 targetPos = _target.position;

        Vector2 delta = targetPos - myPos;

        if (delta.sqrMagnitude > 0) // Need to move cam
        {
            Vector3 newPos = Vector2.MoveTowards(myPos, targetPos, _camSpeed * a_fDeltaTime);
            newPos.z = myPos.z;
            _myTransform.position = newPos;
        }
    }
}
