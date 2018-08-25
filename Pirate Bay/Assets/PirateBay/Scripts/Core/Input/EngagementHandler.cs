using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EngagementType
{
    Keyboard,
    Controller
}

public struct EngagementStatus
{
    public bool KeyboardEngaged;
    public bool[] ControllersEngaged;

    public EngagementStatus(int a_controllers)
    {
        KeyboardEngaged = false;
        ControllersEngaged = new bool[a_controllers];
    }
}

public class EngagementHandler
{
    private List<InputPlayer> _players;

    private EngagementStatus currentEngagmentStatus;
    private Action<EngagementStatus> _sendStatusAction;
    private InputButtonValue engagementKey;

    private KeyCode keyboardEngagementKey;
    private GamePadKeyCode gamePadEngagementKey;

    public EngagementHandler(InputButtonValue a_engagementKey, ref List<InputPlayer> a_players, Action<EngagementStatus> a_sendStatusAction)
    {
        engagementKey = a_engagementKey;
        _players = a_players;
        _sendStatusAction = a_sendStatusAction;

        string[] connectedControllers = Input.GetJoystickNames();
        int numControllers = connectedControllers.Length;
        currentEngagmentStatus = new EngagementStatus(numControllers);

        keyboardEngagementKey = InputManager.Instance.KeyboardDefaults().Action1;
        gamePadEngagementKey = InputManager.Instance.GamePadDefaults().Action1;
    }

    public void UpdateEngagement()
    {
        bool allEngaged = true;
        for (int i = 0; i < _players.Count; ++i)
        {
            if (!_players[i].isEngaged)
            {
                allEngaged = false;
                CheckPlayerEngagement(_players[i]);
                break;
            }
        }

        if (allEngaged)
            _sendStatusAction(currentEngagmentStatus);

    }

    private void CheckPlayerEngagement(InputPlayer a_player)
    {
        if (!currentEngagmentStatus.KeyboardEngaged)
        {
            if (CheckKeyboardEngagement())
            {
                EngagePlayer(a_player, EngagementType.Keyboard);
                return;
            }
        }

        if (currentEngagmentStatus.ControllersEngaged.Length > 0)
        {
            int id;
            if (CheckGamePadEngagement(out id))
            {
                EngagePlayer(a_player, EngagementType.Controller, id);
                return;
            }
        }
            
    }

    private bool CheckKeyboardEngagement()
    {
        if (Input.GetKeyDown(keyboardEngagementKey))
        {
            // Keyboard Engaged
            currentEngagmentStatus.KeyboardEngaged = true;
            Debug.Log("Keyboard Engaged");
            return true;
        }

        return false;
    }

    private bool CheckGamePadEngagement(out int a_controllerID)
    {
        for (int i = 0; i < currentEngagmentStatus.ControllersEngaged.Length; ++i)
        {
            if (currentEngagmentStatus.ControllersEngaged[i])
                continue;

            KeyCode padKeycode = GamePadKeyCodeHelper.GetTargetPadButton(gamePadEngagementKey, i);
            if (Input.GetKeyDown(padKeycode))
            {
                // Pad Engaged
                currentEngagmentStatus.ControllersEngaged[i] = true;
                Debug.Log("Pad " + i + " engaged!");
                a_controllerID = i;
                return true;
            }
        }

        a_controllerID = -1;
        return false;
    }

    private void EngagePlayer(InputPlayer a_player, EngagementType a_type, int id = -1)
    {
        a_player.Engage(a_type, id);
    }


}