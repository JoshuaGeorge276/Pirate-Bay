using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

[RequireComponent(typeof(ActorMovement))]
public class PlayerInput : ManagedBehaviour
{
    InputPlayer inputPlayer;

    ActorMovement movement;

    protected override void Awake()
    {
        base.Awake();

        movement = GetComponent<ActorMovement>();
    }

    protected override void Start()
    {
        base.Start();
        inputPlayer = InputManager.Instance.GetPlayer(0);
    }

    public override void ManagedUpdate(float a_fDeltaTime)
    {
        if (!inputPlayer.isEngaged)
            return;

        UpdateMovement();
    }

    private void UpdateMovement()
    {
        float x = inputPlayer.Controller.Device.GetAxis(InputAxisValue.LeftX);
        float y = inputPlayer.Controller.Device.GetAxis(InputAxisValue.LeftY);

        Vector2 input;
        input.x = x;
        input.y = y;
        movement.Input = input;
    }
}
