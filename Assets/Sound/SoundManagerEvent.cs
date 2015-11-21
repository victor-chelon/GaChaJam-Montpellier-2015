using UnityEngine;
using System.Collections;

/*
 * Comment émettre un event:
		SoundManagerEvent.emit(SoundManagerType.DRINK);
 * 
 * Comment traiter un event (dans un start ou un initialisation)
		EventManagerScript.onEvent += (EventManagerType emt, GameObject go) => {
			if (emt == EventManagerType.ENEMY_HIT)
			{
				//SpawnFXAt(go.transform.position);
			}
		};
 * ou:
		void maCallback(EventManagerType emt, GameObject go) => {
			if (emt == EventManagerType.ENEMY_HIT)
			{
				//SpawnFXAt(go.transform.position);
			}
		};
		EventManagerScript.onEvent += maCallback;
 * 
 * qui permet de:
		EventManagerScript.onEvent -= maCallback; //Retire l'appel
 */

public class SoundManagerEvent : SingletonScript<SoundManagerEvent>
{

    public delegate void EventAction(int index);

    public static event EventAction onEvent;
    private SoundManager _soundManager;

    void Start()
    {
        //  onEvent += (emt) => { Debug.Log("&"); };
        _soundManager = GetComponent<SoundManager>();
    }

    public static void Emit(int index)
    {

        if (onEvent != null)
        {
            onEvent(index);
        }
    }

    public void Volume(int index, float vol)
    {
        _soundManager.Volume(index, vol);
    }
}