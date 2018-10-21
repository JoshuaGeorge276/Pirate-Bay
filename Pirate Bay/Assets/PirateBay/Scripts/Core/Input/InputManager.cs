using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Core;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;

public class InputManager : SingletonBehaviour<InputManager>
{
    [Range(1, 4)]
    [SerializeField]
    private int playerCount;

    [System.Serializable]
    private class DefaultBindings
    {
        public KeyboardBindings keyboardDefaults;
        public GamePadBindings gamepadDefaults;
    }

    [SerializeField] 
    private DefaultBindings _defaultBindings;

    public KeyboardBindings KeyboardDefaults()
    {
        return _defaultBindings.keyboardDefaults;
    }

    public GamePadBindings GamePadDefaults()
    {
        return _defaultBindings.gamepadDefaults;
    }

    private InputPlayer _primaryInputPlayer;

    private List<InputPlayer> players = new List<InputPlayer>();

    private float _deltaTime;

    private Action currentUpdateAction;

    private EngagementHandler engagementHandler;
    private EngagementStatus? currentEngagementStatus = null;

    private bool isDebug = false;
    public bool checkEngagementOnStart = false;
    public InputButtonValue engagementKey;

    public void SetDebugMode(bool a_value) { isDebug = a_value; }

    protected override void Awake()
    {
        base.Awake();

        currentUpdateAction = UpdatePlayers;

        _primaryInputPlayer = AddPlayer();

        if (playerCount > 1)
        {
            for (int i = 1; i < playerCount; i++)
            {
                AddPlayer();
            }
        }

        string[] names = Input.GetJoystickNames();
        for (int i = 0; i < names.Length; ++i)
        {
            Debug.Log(names[i]);
        }

        if (checkEngagementOnStart)
        {
            StartEngagement(true);
        }
    }
	
	// Update is called once per frame
	private void Update ()
	{
        if(currentUpdateAction != null)
            currentUpdateAction();
	}

    // isPrimaryEngagement - only checking for main player
    private void StartEngagement(bool isPrimaryEngagement)
    {
        List<InputPlayer> engagementsToCheck;

        if (isPrimaryEngagement)
        {
            engagementsToCheck = new List<InputPlayer>();
            engagementsToCheck.Add(_primaryInputPlayer);
        }
        else
        {
            engagementsToCheck = players;
        }
        
        engagementHandler = new EngagementHandler(
            engagementKey,
            ref players,
            EndEngagement
            );

        currentUpdateAction = CheckForEngagement;

        Debug.Log("Press a button to engage a controller!");
    }

    private void CheckForEngagement()
    {
        if(engagementHandler != null)
            engagementHandler.UpdateEngagement();
    }

    private void EndEngagement(EngagementStatus a_status)
    {
        currentEngagementStatus = a_status;
        engagementHandler = null;
        currentUpdateAction = UpdatePlayers;
    }

    private void UpdatePlayers()
    {
        _deltaTime = Time.deltaTime;
        for (int i = 0; i < playerCount; ++i)
        {
            players[i].Update(_deltaTime);
        }

        bool action1 = GetPlayer(0).Controller.Device.GetButtonDown(InputButtonValue.Action1);
        bool action2 = GetPlayer(0).Controller.Device.GetButtonDown(InputButtonValue.Action2);
        bool action3 = GetPlayer(0).Controller.Device.GetButtonDown(InputButtonValue.Action3);
        bool action4 = GetPlayer(0).Controller.Device.GetButtonDown(InputButtonValue.Action4);

        //Debug.Log("Horizontal: " + Input.GetAxis("KeyboardHorizontal"));

        if(action1)
            Debug.Log("Action 1 pressed!");
        
        if(action2)
            Debug.Log("Action 2 pressed!");
        
        if(action3)
            Debug.Log("Action 3 pressed!");
        
        if(action4)
            Debug.Log("Action 4 pressed!");
    }

    private void LateUpdate()
    {
        for (int i = 0; i < playerCount; ++i)
        {
            players[i].LateUpdate(_deltaTime);
        }
    }

    public InputPlayer AddPlayer()
    {
        InputPlayer newPlayer = new InputPlayer();
        players.Add(newPlayer);
        return newPlayer;
    }

    public InputPlayer GetPrimaryPlayer()
    {
        return _primaryInputPlayer;
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
