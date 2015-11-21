using UnityEngine;
using System.Collections;

public class AnimationsManager : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeLevel()
	{
		GameManager.Instance.CheckNextLevel();
	}
}
