using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Memorama.General
{
    public sealed class GeneralMethods
    {

        public static T GetIntance<T>(string name) where T : Behaviour
        {
            T[] managers = GameObject.FindObjectsOfType<T>();
            if (managers == null || managers.Length <= 0)
            {
                return new GameObject(name).AddComponent<T>();
            }
            else
            {
                for (int i = 1; i < managers.Length; ++i)
                    managers[i].enabled = false;
                return managers[0];
            }
        }
    }
}