using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public sealed class ControllerManager : SingletonScript<ControllerManager>
{
    private static bool hasInstantiateManagers = false;

    [SerializeField]private List<GameObject> _managerList = new List<GameObject>(); 

    private void Awake()
    {
        if (!hasInstantiateManagers)
        {
            foreach (GameObject manager in _managerList)
            {
                GameObject managerGameObject = Instantiate(manager);
                DontDestroyOnLoad(managerGameObject);
            }
        }
    }
}