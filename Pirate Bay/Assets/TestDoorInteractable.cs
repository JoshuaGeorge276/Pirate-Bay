using System.Collections;
using System.Collections.Generic;
using Core;
using PirateBay.World;
using UnityEngine;

[RequireComponent(typeof(WorldObject))]
public class TestDoorInteractable : ManagedBehaviour, Interactable
{
    private WorldObject _worldObject;

    protected override void Awake()
    {
        _worldObject = GetComponent<WorldObject>();
        _worldObject.OnEnabled += RegisterInteractable;
        _worldObject.OnDisabled += UnregisterInteractable;
    }

    private void RegisterInteractable()
    {
        World.Instance.RegisterInteractable(_worldObject.GetHandle(), this);
    }

    private void UnregisterInteractable()
    {
        World.Instance.UnregisterInteractable(_worldObject.GetHandle());
    }

    bool Interactable.CanInteract()
    {
        return true;
    }

    void Interactable.Interact()
    {
        Debug.Log("TestDoorInteractable Interacted with!");
    }
}
