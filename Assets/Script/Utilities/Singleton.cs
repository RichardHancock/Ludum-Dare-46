using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; private set; }

        public static bool IsInit => Instance != null;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Tried to init more than one instance of a Singleton class");
            }
            else
            {
                Instance = (T)this;
            }
        }

        protected virtual void OnDestroy()
        {
            //Clear the singleton reference if 'this' instance matches (Should always match)
            if (Instance == this)
                Instance = null;
        }
    }
}
