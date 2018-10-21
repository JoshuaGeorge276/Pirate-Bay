using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class GamingEnvironment : MonoBehaviour
{
    public bool _debugMode = false;

    [SerializeField]
    private List<GameObject> _environmentObjects;

    void Awake()
    {
        GamingEnvironment environment = FindObjectOfType<GamingEnvironment>();

        if(environment && environment != this)
        {
            Debug.Log("Duplicate Gaming Environment Detected, destroying" + gameObject.name);
            Destroy(this);
            return;
        }

        for(int i = 0; i < _environmentObjects.Count; ++i)
        {
            Instantiate(_environmentObjects[i], transform);
        }

        if(InputManager.Instance)
            InputManager.Instance.SetDebugMode(_debugMode);
    }
}
