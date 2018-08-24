using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class InputManager : SingletonBehaviour<InputManager>
{
    [SerializeField]
    private int playerCount;

    private List<InputPlayer> players = new List<InputPlayer>();

    private float _deltaTime;

    private void Awake()
    {
        for (int i = 0; i < playerCount; i++)
        {
            players.Add(new InputPlayer());
        }
    }

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _deltaTime = Time.deltaTime;
	    for (int i = 0; i < playerCount; ++i)
	    {
            players[i].Update(_deltaTime);
	    }

	    //float x = GetPlayer(0).Controller.Device.GetAxis(InputAxisValue.LeftX);
	    //float y = GetPlayer(0).Controller.Device.GetAxis(InputAxisValue.LeftY);
	    //Debug.Log("Horizontal: " + (int)x + " Vertical: " + (int)y);

	    bool action1, action2, action3, action4;

	    action1 = GetPlayer(0).Controller.Device.GetButtonDown(InputButtonValue.Action1);
	    action2 = GetPlayer(0).Controller.Device.GetButtonDown(InputButtonValue.Action2);
	    action3 = GetPlayer(0).Controller.Device.GetButtonDown(InputButtonValue.Action3);
	    action4 = GetPlayer(0).Controller.Device.GetButtonDown(InputButtonValue.Action4);



        if(action1)
            Debug.Log("Action 1 pressed!");
        
	    if(action2)
	        Debug.Log("Action 2 pressed!");
        
	    if(action3)
	        Debug.Log("Action 3 pressed!");
        
	    if(action4)
	        Debug.Log("Action 4 pressed!");
	}

    void LateUpdate()
    {
        for (int i = 0; i < playerCount; ++i)
        {
            players[i].LateUpdate(_deltaTime);
        }
    }

    public InputPlayer GetPlayer(int a_id)
    {
        if (a_id < playerCount)
        {
            return players[a_id];
        }

        return null;
    }
}
