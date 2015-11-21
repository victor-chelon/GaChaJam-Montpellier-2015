using UnityEngine;
using System.Collections;

public class ParticleEmit : MonoBehaviour 
{
    [SerializeField] private Color _colorBase;
    [SerializeField] private Color _colorEnd;

    [SerializeField] private bool _activeParticle = true;
    [SerializeField] private bool _activeWallHit;

	[SerializeField] private float _beginOffset;
    [SerializeField] private float _rate;
    [SerializeField] private float _offsetDegre;

    private ParticleManager _particuleManager;
    private float _rateFusion;


    [Range(10f, 360f)]
    [SerializeField] private float _particleByAngle;

    [SerializeField] private GameObject _lightParticule;
    

	private GameManager gaMana;
    private Light _light;
    private Color _currontColor;
    private float _time;
    private float _rateTime;
    private bool _fusion;
	private bool _pauseStarted;
	private Coroutine _coroutine;

    // Use this for initialization
	void Start () 
    {
		_time = 1 / _rate;
		gaMana = GameManager.Instance;
		_particuleManager = ParticleManager.Instance;

        _light = GetComponent<Light>();
        _currontColor = _colorBase;
//        StartCoroutine(StartEmission());

        _light.color = _currontColor;
	}


	// Update is called once per frame
	void Update ()
    {
		if (gaMana.CurrentState == GameManager.GameStates.Pause && _pauseStarted)
		{
			StopCoroutine(_coroutine);
			_pauseStarted = false;
		}
		else if (gaMana.CurrentState != GameManager.GameStates.Pause && !_pauseStarted)
		{
			_coroutine = StartCoroutine(StartEmission());
			_pauseStarted = true;
		}

		if (transform.position.x > 5.2f)
			transform.position = new Vector3(5.2f, transform.position.y, transform.position.z);
		else if (transform.position.x < -5.2f)
			transform.position = new Vector3(-5.2f, transform.position.y, transform.position.z);
		
		if (transform.position.y > 4.8f)
			transform.position = new Vector3(transform.position.x, 4.8f, transform.position.z);
		else if (transform.position.y < -4.8f)
			transform.position = new Vector3(transform.position.x, -4.8f, transform.position.z);
	}

    void SetParticule(GameObject lightParticule, float degre)
    {
        lightParticule.tag = "Particle" + tag;
        lightParticule.transform.position = transform.position;
        lightParticule.transform.Rotate(Vector3.forward * degre);

        LightParticle lParticule = lightParticule.GetComponent<LightParticle>();
        lParticule.SetColor(_currontColor, _colorEnd);
        lParticule.SetWallHit(_activeWallHit);
        lParticule.SetFusion(_fusion);
		lightParticule.transform.SetParent(_particuleManager.transform);
    }

    void CreateParticule()
    {
        for (int i = 0; i < 360 / _particleByAngle; ++i)
        {
            GameObject lightParticule = Instantiate(_lightParticule);

            SetParticule(lightParticule, _particleByAngle + _particleByAngle * i + _offsetDegre);
        }

		if (tag == "Player")
			SoundManagerEvent.Emit(7);
		else
			SoundManagerEvent.Emit(8);
	}
	
	void CreateFusionParticule()
    {
        for (int i = 0; i < 360 / _particuleManager.ParticleByAngleFusion; ++i)
        {
            GameObject lightParticule = Instantiate(_lightParticule);

            SetParticule(lightParticule, _particuleManager.ParticleByAngleFusion + _particuleManager.ParticleByAngleFusion * i + _particuleManager.OffsetDegreFusion + _offsetDegre);
        }

		if (tag == "Player")
			SoundManagerEvent.Emit(7);
		else
			SoundManagerEvent.Emit(8);
	}
	
	public Color GetColor()
    {
        return _colorBase;
    }

    IEnumerator StartEmission()
    {
		yield return new  WaitForSeconds(_beginOffset);

        while(_activeParticle)
        {
            _time += Time.deltaTime;

            if (!_fusion)
            {
                if (_time >= 1/_rate)
                {
                    _time = 0f;
                    CreateParticule();
                }
            }
            else
            {
                if (_time >= 1 / _particuleManager.RateFusion)
                {
                    _time = 0f;
                    CreateFusionParticule();
                }
            }
                
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Player2")
        {
            ParticleEmit p2 = other.gameObject.GetComponent<ParticleEmit>();
            _currontColor = p2.GetColor() + _colorBase;
            _fusion = true;

			if (tag == "Player")
				SoundManagerEvent.Emit(9);
			
			if (gaMana.CurrentState == GameManager.GameStates.MainMenu)
				gaMana.ShowCredit(true);
		}
	}
	
	void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Player2")
        {
            _currontColor = _colorBase;
            _fusion = false;

			if (gaMana.CurrentState == GameManager.GameStates.MainMenu)
				gaMana.ShowCredit(false);
			
			StopCoroutine(_coroutine);
			_coroutine = StartCoroutine(StartEmission());
			
			_time = 1/_rate;
		}
	}
}
