﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTypes : MonoBehaviour {

	public enum Type
    {
        Greeting,
        GreetingResponse,
        Plead,
        RequestToJoinCrew,
        Threat,
        Goodbye
    }

    public enum Triggers
    {
        PlayerConvoIntro
    }
}
