using UnityEngine;

namespace Core
{
    public class SingletonBehaviour<T> : MonoBehaviour
        where T : Component
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Debug.LogWarning("Destroyed " + gameObject.name +
                                 "Due to duplicate singleton instance already existing!");
                Destroy(gameObject);
            }
        }
    }

    public class PersistantSingletonBehaviour<T> : MonoBehaviour
        where T : Component
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}