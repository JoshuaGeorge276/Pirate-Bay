using System.Collections;
using System.Collections.Generic;
using Core.EventSystem;
using UnityEngine;

namespace PirateBay.Events.Camera
{
    public static class CamEvents
    {
        public class SetCamTargetEvent : GameEvent
        {
            public SetCamTargetEvent(Transform a_target)
            {
                target = a_target;
            }
            public Transform target;
        }
    }
}
