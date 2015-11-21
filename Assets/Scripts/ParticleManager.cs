using UnityEngine;
using System.Collections;

public class ParticleManager : SingletonScript<ParticleManager>
{

    [SerializeField]
    private float _lifePlayer1;
    [SerializeField]
    private float _lifePlayer2;

    [SerializeField] private bool _wallHitFusion;
    [Range(10f, 360f)]
    [SerializeField]
    private float _particleByAngleFusion;

    [SerializeField] private float _offsetDegreFusion;
    [SerializeField] private float _rateFusion;
    [SerializeField] private float _lifeFusion;
    [SerializeField] private float _speedFusion;

    private bool _activeLife1;
    private bool _activeLife2;


    private float _lifeP1;
    private float _lifeP2;
    private float _timeP1 = 0f;
    private float _timeP2 = 0f;


    #region properties

    public float LifeFusion
    {
        get { return _lifeFusion; }
    }

    public float SpeedFusion
    {
        get { return _speedFusion; }
    }

    public bool WallHitFusion
    {
        get { return _wallHitFusion; }
    }

    public float ParticleByAngleFusion
    {
        get { return _particleByAngleFusion; }
    }

    public float OffsetDegreFusion
    {
        get { return _offsetDegreFusion; }
    }

    public float RateFusion
    {
        get { return _rateFusion; }
    }

    public float Life1
    {
        get { return _lifeP1; }
    }

    public float Life2
    {
        get { return _lifeP2; }
    }
#endregion
    // Use this for initialization
	void Start ()
	{
	    _lifeP1 = _lifePlayer1;
	    _lifeP2 = _lifePlayer2;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (_lifeP1 <= 0)
	    {
	        _activeLife1 = false;
            _lifeP1 = _lifePlayer1;
	        _timeP1 = 0f;
	    }

	    if (_lifeP2 <= 0)
	    {
	        _activeLife2 = false;
            _lifeP2 = _lifePlayer2;
            _timeP2 = 0f;
	    }

	    if (_activeLife1)
	    {
	        _timeP1 += Time.deltaTime;
	        _lifeP1 = _lifePlayer1 - _timeP1;
	    }

	    if (_activeLife2)
	    {
            _timeP2 += Time.deltaTime;
            _lifeP2 = _lifePlayer2 - _timeP2;
	    }
	}

    public void StartLifeParticule(string tage)
    {
        if (tage == "Player")
            _activeLife1 = true;
        else
            _activeLife2 = true;

    }

    
}
