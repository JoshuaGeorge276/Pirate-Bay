using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public interface EnvironmentModule
{
    void Load();
    void PostLoad();
}

public class GamingEnvironment : MonoBehaviour
{
    private BehaviourManager _behaviourManager;
    private InputManager _inputManager;

    private bool _debugMode = false;

    public GamingEnvironment(bool a_DebugMode)
    {
        _debugMode = a_DebugMode;
    }


    void Awake()
    {
        _behaviourManager = new BehaviourManager();
        _inputManager = new InputManager(_debugMode);

        LoadModules();
    }

	// Use this for initialization
	void Start () 
	{
	    
	}

    private void Update()
    {
        _behaviourManager.Update();
    }

    private void FixedUpdate()
    {
        _behaviourManager.FixedUpdate();
    }

    private void LateUpdate()
    {
        _behaviourManager.LateUpdate();
    }

    private void LoadModules()
    {
        _behaviourManager.Load();
        _inputManager.Load();

        _behaviourManager.PostLoad();
        _inputManager.PostLoad();
    }
}
