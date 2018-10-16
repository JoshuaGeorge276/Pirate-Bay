using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PirateBay.World
{
    public class WorldInteractableManager
    {
        private Dictionary<int, Interactable> _worldInteractables;

        public WorldInteractableManager()
        {
            _worldInteractables = new Dictionary<int, Interactable>();
        }

        private bool isRegisteredInteractable(int a_handle)
        {
            return _worldInteractables.ContainsKey(a_handle);
        }

        public bool TryGetInteractable(int a_handle, out Interactable o_interactable)
        {
            return _worldInteractables.TryGetValue(a_handle, out o_interactable);
        }

        public void RegisterInteractable(int a_handle, Interactable a_interactable)
        {
            if (isRegisteredInteractable(a_handle))
                return;

            _worldInteractables.Add(a_handle, a_interactable);
        }

        public void UnregisterInteractable(int a_handle)
        {
            if (!isRegisteredInteractable(a_handle))
                return;

            _worldInteractables.Remove(a_handle);
        }
    }
}


