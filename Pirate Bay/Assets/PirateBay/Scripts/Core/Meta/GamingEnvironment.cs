using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class GamingEnvironment : MonoBehaviour
{
    public bool _debugMode = false;

    [SerializeField]
    private GameObject[] environmentObjects;

    void Awake()
    {
        GamingEnvironment environment = FindObjectOfType<GamingEnvironment>();

        if(environment && environment != this)
        {
            Destroy(this);
            return;
        }

        for(int i = 0; i < environmentObjects.Length; ++i)
        {
            Instantiate(environmentObjects[i], transform);
        }

        if(InputManager.Instance)
            InputManager.Instance.SetDebugMode(_debugMode);
    }

	// Use this for initialization
	void Start () 
	{
	    
	}
}
