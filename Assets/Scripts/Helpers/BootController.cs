using UnityEngine;
using System.Collections;

public sealed class BootController : MonoBehaviour
{
    [SerializeField] private GameObject _controllerManager;
    private static bool hasBeenBooted = false;

    private void Awake()
    {
        if (!hasBeenBooted)
        {
            GameObject managerInstancier = Instantiate(_controllerManager);

            if (null == managerInstancier)
            {
                Debug.LogError("There is no ManagerInstancier For The Following Path: Prefabs/Manager/ManagerInstancier");
                return;
            }

            DontDestroyOnLoad(managerInstancier);
            hasBeenBooted = true;
        }
        DestroyImmediate(gameObject);
    }

}
