using UnityEngine;
using System.Collections;

public class SingletonScript<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool onApplicationQuit = false;

    private static T instance;
    public static T Instance
    {
        get
        {
            if (onApplicationQuit)
            {
                return null;
            }

            if (null == instance)
            {

                instance = GameObject.FindObjectOfType<T>();
                T[] instances = GameObject.FindObjectsOfType<T>();

                if (instances.Length > 1)
                {
                    Debug.LogError("Something got wrong, because you have several singleton for the following type: " + typeof(T));
                    return instance;
                }


                //this part of code must remain in comment, nobody can instantiate a singleton with a getter, only by instantiating it.
                /*  if (null == instance)
                  {
                      Debug.LogError("NULL");
                      GameObject singleton = new GameObject();
                      singleton.name = "Singleton of type : " + typeof(T);

                      instance = singleton.AddComponent<T>();

                      DontDestroyOnLoad(singleton);
                  }
                  else
                  {

                  }*/
            }

            return instance;
        }
    }

    private void onDestroy()
    {
        onApplicationQuit = true;
    }
}