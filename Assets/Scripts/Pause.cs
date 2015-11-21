using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour
{
    [SerializeField] private float _timeToPlay;
    [SerializeField] private float _timeToQuit;


    private GameManager gaMana;
    private bool _pauseStarted;
    [SerializeField]
    private GameObject _buttonPlay;
    [SerializeField]
    private GameObject _buttonQuit;
    [SerializeField]
    private GameObject _layerPause;
    private GameObject Player1;
    private GameObject Player2;

    private float _time;
    // Use this for initialization
    void Start ()
	{
        gaMana = GameManager.Instance;
    }

    // Update is called once per frame
    void Update () 
    {
	    if (gaMana.CurrentState == GameManager.GameStates.Pause)
	    {
	        if (!_pauseStarted)
	        {
	            CreatePausePLayer();

                _layerPause.SetActive(true);

                _pauseStarted = true;
	        }
			if (checkCollision(Player1, _buttonPlay) || checkCollision(Player2, _buttonPlay))
				_buttonPlay.GetComponentInChildren<Animator>().SetBool("hover",true);
			else
				_buttonPlay.GetComponentInChildren<Animator>().SetBool("hover",false);

			if (checkCollision(Player1, _buttonQuit) || checkCollision(Player2, _buttonQuit))
			    _buttonQuit.GetComponentInChildren<Animator>().SetBool("hover",true);
			else
			    _buttonQuit.GetComponentInChildren<Animator>().SetBool("hover",false);


	        if (checkCollision(Player1, _buttonPlay) && checkCollision(Player2, _buttonPlay))
			{
	            OverButtonPlay();
				_buttonPlay.GetComponent<Animator>().SetInteger("NumbPlayerOn",2);
			}
            else if (checkCollision(Player1, _buttonQuit) && checkCollision(Player2, _buttonQuit))
			{
	            OverButtonQuit();
				_buttonQuit.GetComponent<Animator>().SetInteger("NumbPlayerOn",2);
			}
			else if (checkCollision(Player1, _buttonPlay) && !checkCollision(Player2, _buttonPlay))
			{
				_buttonPlay.GetComponent<Animator>().SetInteger("NumbPlayerOn",1);
				_time = 0f;
			}
			else if (!checkCollision(Player1, _buttonPlay) && checkCollision(Player2, _buttonPlay))
			{
				_buttonPlay.GetComponent<Animator>().SetInteger("NumbPlayerOn",1);
				_time = 0f;
			}
			else if (checkCollision(Player1, _buttonQuit) && !checkCollision(Player2, _buttonQuit))
			{
				_buttonQuit.GetComponent<Animator>().SetInteger("NumbPlayerOn",1);
				_time = 0f;
			}
			else if (!checkCollision(Player1, _buttonQuit) && checkCollision(Player2, _buttonQuit))
			{
				_buttonQuit.GetComponent<Animator>().SetInteger("NumbPlayerOn",1);
				_time = 0f;
			}
			else
			{
				_buttonPlay.GetComponent<Animator>().SetInteger("NumbPlayerOn",0);
				_buttonQuit.GetComponent<Animator>().SetInteger("NumbPlayerOn",0);
	            _time = 0f;
			}

        }
	    else
	    {
	        if (_pauseStarted)
	        {
                Destroy(Player1);
                Destroy(Player2);

                _layerPause.SetActive(false);

                _pauseStarted = false;
	        }
	    }
	}

    void CreatePausePLayer()
    {
        Player1 = Instantiate(gaMana.PrefabsPlayer1);
        Player2 = Instantiate(gaMana.PrefabsPlayer2);
        Player1.name = "Player1Pause";
        Player2.name = "Player2Pause";

		PlayerManager pM1 = Player1.GetComponent<PlayerManager>();
		PlayerManager pM2 = Player2.GetComponent<PlayerManager>();

        pM1.ActiveInPause = true;
		pM1.OtherPlayer = pM2;
        pM2.ActiveInPause = true;
		pM2.OtherPlayer = pM1;

        Player1.transform.position = new Vector3(Player1.transform.position.x, Player1.transform.position.y, -2.2f);
        Player2.transform.position = new Vector3(Player2.transform.position.x, Player2.transform.position.y, -2.2f);
    }

    void OverButtonPlay()
    {
        _time += Time.deltaTime;

        if (_time >= _timeToPlay)
        {
            gaMana.Pause();
            _time = 0f;
        }
    }

    void OverButtonQuit()
    {
        _time += Time.deltaTime;

        if (_time >= _timeToQuit)
        {
			if(Application.loadedLevel != 1)
				Application.LoadLevel(1);
			else
				Application.Quit();

            _time = 0f;
        }
    }

    bool checkCollision(GameObject pointer, GameObject box)
    {
        BoxCollider bo = box.GetComponent<BoxCollider>();

        float sideX = bo.size.x * box.transform.localScale.x;
        float sideY = bo.size.y * box.transform.localScale.y;

        float offsetX = bo.center.x * box.transform.localScale.x;
        float offsetY = bo.center.y * box.transform.localScale.y;


        if (pointer.transform.position.x < box.transform.position.x + sideX + offsetX &&
            pointer.transform.position.x > box.transform.position.x - sideX + offsetX &&
            pointer.transform.position.y < box.transform.position.y + sideY + offsetY &&
            pointer.transform.position.y > box.transform.position.y - sideY + offsetY)
        {
            return true;
        }

        return false;
    }

}
