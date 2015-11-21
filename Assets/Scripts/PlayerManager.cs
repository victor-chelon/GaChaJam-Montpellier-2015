using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	private enum PlayerSides {Player1, Player2};
	[SerializeField] private PlayerSides playerSide;

	[SerializeField]private bool _activeInPause ;

	[SerializeField] private float _impulseByTime;
	[SerializeField] private PlayerManager _otherPlayer;
	private float _time;
	
	public bool ActiveInPause
	{
		set { _activeInPause = value; }
	}

	public PlayerManager OtherPlayer
	{
		set { _otherPlayer = value; }
	}

    private int m_PlayerId;

	private float _damage;

    //Is the player using GamePad (or Keyboard)
    private bool m_IsUsingGamePad;

    //Inputs
    private float m_XInput;
    private float m_YInput;
	private float m_RXInput;
	private float m_RYInput;

    public float m_LightMoveForce;
    public float m_LightMaxSpeed;
    public float m_LightDecceleration;

    public float m_EmitterMoveForce;
    public float m_EmitterMaxSpeed;
    public float m_EmitterDecceleration;

    //RigidbodyHelper
    private Rigidbody m_Rigidbody;
	private Rigidbody m_ParticleRigidbody;

    private Vector3 m_MovementAxis;
	private Vector3 m_ParticleMovementAxis;


    private ContactPoint[] colPoints;

	private GameManager gaMana;

	private bool lifeLost;

	// Use this for initialization
	void Start () 
	{
		gaMana = GameManager.Instance;
        m_Rigidbody = this.GetComponent<Rigidbody>();

		_damage = transform.localScale.x * 0.1f;

        //PlayerDetection
        if (this.playerSide == PlayerSides.Player1)
		{
            m_PlayerId = 1;
			m_ParticleRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
			InvokeRepeating("DecreaseLife", 3,gaMana.timeSpan * 0.1f);
			InvokeRepeating("DecreaseLifePlayer", 3,gaMana.timeSpan * 0.1f);
		}
        else if (this.playerSide == PlayerSides.Player2)
		{
            m_PlayerId = 2;
			m_ParticleRigidbody = GameObject.FindGameObjectWithTag("Player2").GetComponent<Rigidbody>();
			InvokeRepeating("DecreaseLifePlayer", 3,gaMana.timeSpan * 0.1f);
		}


    }
	
	// Update is called once per frame

    void Update()
    {
		if (gaMana.CurrentState != GameManager.GameStates.Pause || _activeInPause)
		{
			InputDetection();
			m_MovementAxis = new Vector3(m_XInput, -m_YInput, 0.0f);
			m_ParticleMovementAxis = new Vector3(m_RXInput, -m_RYInput, 0.0f);

			_time += Time.deltaTime;
			if (_time >= _impulseByTime && m_PlayerId == 1)
			{
				float dist = Vector2.Distance(transform.position, _otherPlayer.transform.position);
				
				SoundManagerEvent.Instance.Volume(0, 1 - dist / 14);
				SoundManagerEvent.Emit(6);
				_time = 0f;
			}
		}
		else
		{
			m_MovementAxis = new Vector3(0f, 0f, 0.0f);
			m_ParticleMovementAxis = new Vector3(0, 0, 0.0f);
		}

    }


    void FixedUpdate()
    {
		//Light Core movement
        m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, m_LightMaxSpeed);
        m_Rigidbody.AddForce(m_MovementAxis.normalized * m_LightMoveForce);

        m_Rigidbody.velocity *= m_LightDecceleration;

		//Particle Emitter movement
		m_ParticleRigidbody.velocity = Vector3.ClampMagnitude(m_ParticleRigidbody.velocity, m_EmitterMaxSpeed);
		m_ParticleRigidbody.AddForce(m_ParticleMovementAxis.normalized * m_EmitterMoveForce);
		
		m_ParticleRigidbody.velocity *= m_EmitterDecceleration;

    }


void InputDetection()
    {
        #region Gamepad
        if (Input.GetAxis("LHorizontalP" + m_PlayerId) != 0)
        {
            m_XInput = Input.GetAxis("LHorizontalP" + m_PlayerId);
            if (m_IsUsingGamePad == false)
            {
                m_IsUsingGamePad = true;
            }
        }
        else
        {
            m_XInput = 0;

        }
        
        if (Input.GetAxis("LVerticalP" + m_PlayerId) != 0)
        {
            m_YInput = Input.GetAxis("LVerticalP" + m_PlayerId);
            if (m_IsUsingGamePad == false)
            {
                m_IsUsingGamePad = true;
            }
        }
        else
        {
            m_YInput = 0;
        }

		//Right Stick Controls
		if (Input.GetAxis("RHorizontalP" + m_PlayerId) != 0)
		{
			m_RXInput = Input.GetAxis("RHorizontalP" + m_PlayerId);
			if (m_IsUsingGamePad == false)
			{
				m_IsUsingGamePad = true;
			}
		}
		else
		{
			m_RXInput = 0;			
		}
		
		if (Input.GetAxis("RVerticalP" + m_PlayerId) != 0)
		{
			m_RYInput = Input.GetAxis("RVerticalP" + m_PlayerId);
			if (m_IsUsingGamePad == false)
			{
				m_IsUsingGamePad = true;
			}
		}
		else
		{
			m_RYInput = 0;
		}
        #endregion

        #region Keyboard
        /*if (Input.GetKey(KeyCode.Z))
        {
            m_YInput = -1;
            if (m_IsUsingGamePad == true)
            {
                m_IsUsingGamePad = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            m_YInput = 0;
        }

        if (Input.GetKey(KeyCode.S))
        {
            m_YInput = 1;
            if (m_IsUsingGamePad == true)
            {
                m_IsUsingGamePad = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            m_YInput = 0;
        }

        //Left/Right
        if (Input.GetKey(KeyCode.Q))
        {
            m_XInput = -1;
            if (m_IsUsingGamePad == true)
            {
                m_IsUsingGamePad = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            m_XInput = 0;
        }

        if ( Input.GetKey(KeyCode.D))
        {
            m_XInput = 1;
            if (m_IsUsingGamePad == true)
            {
                m_IsUsingGamePad = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            m_XInput = 0;
        }*/
        #endregion
    }


    void OnCollisionEnter(Collision col)
	{
//		if(col.collider.tag == "Wall")
//		{
//			colPoints = col.contacts;
//			for(int i = 0; i < colPoints.Length; i++)
//			{
//				Debug.Log(colPoints[i].point);
//			}
//		}

		if(m_PlayerId == 1)
		if(col.collider.GetComponent<PlayerManager>()  && !_activeInPause)
		{
			EndLevel();
		}
	}

	void OnCollisionExit(Collision col)
	{
//		if(col.collider.tag == "Wall")
//		{
//			if(col.collider.tag == "Wall")
//			{
//				colPoints = null;
//				blockDirection = BlockDirections.None;
//			}
//		}
	}

//	void CheckColPoint()
//	{
//		for(int i = 0; i < colPoints.Length; i++)
//		{
//			if(this.transform.position.x < colPoints[i].point.x)
//				blockDirection = BlockDirections.Right;
//			else if(this.transform.position.x > colPoints[i].point.x)
//				blockDirection = BlockDirections.Left;
//			
//			if(this.transform.position.y < colPoints[i].point.y)
//				blockDirection = BlockDirections.Up;
//			else if(this.transform.position.y > colPoints[i].point.y)
//				blockDirection = BlockDirections.Down;
//		}
//	}

	void EndLevel()
	{
		gaMana.CheckNextLevel();
	}
	
	void DecreaseLife()
	{
		if(gaMana.CurrentState != GameManager.GameStates.InGame) return;

		if(gaMana.healthPoint > 0)
		{
			lifeLost = true;
			gaMana.healthPoint-=gaMana.timeSpan*0.1f;
			if(gaMana.healthPoint <= 0)
			{
				gaMana.GameOver();
			}
		}
	}
	
	void DecreaseLifePlayer()
	{
		if(gaMana.CurrentState != GameManager.GameStates.InGame) return;

		DamagePlayer();
	}

	void DamagePlayer()
	{
		float x = transform.localScale.x - _damage;
		float y = transform.localScale.y - _damage;

		transform.localScale = new Vector3(x,y,transform.localScale.z);
	}

//	void CheckParticleEmitterPosition()
//	{
//		if(m_ParticleRigidbody.gameObject.transform.position.x > )
//			m_ParticleRigidbody.gameObject.transform.position.Set(
//	}
}
