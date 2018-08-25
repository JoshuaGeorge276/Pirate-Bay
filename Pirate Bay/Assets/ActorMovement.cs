using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class ActorMovement : ManagedBehaviour
{
    private InputPlayer inputPlayer;

    void Awake()
    {

    }

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
	    inputPlayer = InputManager.Instance.GetPlayer(0);
	}

    public override void ManagedUpdate(float a_fDeltaTime)
    {
        if (!inputPlayer.isEngaged)
            return;

        Vector2 currentPos = transform.position;

        float x = inputPlayer.Controller.Device.GetAxis(InputAxisValue.LeftX);
        float y = inputPlayer.Controller.Device.GetAxis(InputAxisValue.LeftY);

        currentPos.x += x * a_fDeltaTime;
        currentPos.y += y * a_fDeltaTime;


        transform.position = currentPos;
    }
}
