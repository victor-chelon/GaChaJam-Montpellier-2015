using UnityEngine;
using System.Collections;

public class LightParticle : MonoBehaviour
{
    [SerializeField] private float _life;
    [SerializeField] private float _speed;
    
    private ParticleManager _particuleManager;
	private GameManager gaMana;

    private float _time;

    private Light _light;
    private Color _color;
    private Color _endColor;
    private bool _wallHit;
    private bool _activeWallHit;
    private bool _fusion;

    private float _currentLife;
    private float _currentSpeed;
    private bool _currentWallHit;


    void Start()
    {
        _light = GetComponent<Light>();
        _light.color = _color;

		gaMana = GameManager.Instance;
		_particuleManager = ParticleManager.Instance;
    }

    void Update()
    {
        if (_fusion)
        {
            _currentLife = _particuleManager.LifeFusion;
            _currentSpeed = _particuleManager.SpeedFusion;
            _currentWallHit = _particuleManager.WallHitFusion;
        }
        else
        {
            _currentLife = _life;
            _currentSpeed = _speed;
            _currentWallHit = _activeWallHit;
        }

		if (gaMana.CurrentState != GameManager.GameStates.Pause)
		{
	        if (!_wallHit)
	        {
	            MoveParticle();
	        }
	        else
	            OnHitWall();

			CheckParticleEmitterPosition();
		}
    }

	void LateUpdate()
	{
	}

    void MoveParticle()
    {
        _time += Time.deltaTime;

        if (_time > _life)
            Destroy(gameObject);

        if (_time >= _life * 0.5f)
        {
            _light.intensity = Mathf.Lerp(_light.intensity, 0f, _currentLife * 4f * Time.deltaTime);
            _light.range = Mathf.Lerp(_light.range, 0f, _currentLife * 4f * Time.deltaTime);

            float x = Mathf.Lerp(transform.localScale.x, 0f, _currentLife * 4f * Time.deltaTime);
            float y = Mathf.Lerp(transform.localScale.y, 0f, _currentLife * 4f * Time.deltaTime);
            transform.localScale = new Vector3(x, y, 0.14f);

            float r = Mathf.Lerp(_light.color.r, _endColor.r, _currentLife * 4f * Time.deltaTime);
            float g = Mathf.Lerp(_light.color.g, _endColor.g, _currentLife * 4f * Time.deltaTime);
            float b = Mathf.Lerp(_light.color.b, _endColor.b, _currentLife * 4f * Time.deltaTime);

            _light.color = new Color(r, g, b);
        }

        transform.Translate(Vector3.up * _currentSpeed * Time.deltaTime);
    }

    void OnHitWall()
    {
        if (tag == "ParticulePlayer")
        {
            if (_particuleManager.Life1 <= 0)
                Destroy(gameObject);
        }
        else if (_particuleManager.Life2 <= 0)
            Destroy(gameObject);
    }

    public void SetColor(Color color, Color endColor)
    {
        _color = color;
        _endColor = endColor;
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Wall")||other.CompareTag("ExteriorWall")) && _currentWallHit)
        {
            _wallHit = true;
            _particuleManager.StartLifeParticule(tag);
        }
    }

    internal void SetFusion(bool fusion)
    {
        _fusion = fusion;
    }

    internal void SetWallHit(bool activeWallHit)
    {
        _activeWallHit = activeWallHit;
    }

	void CheckParticleEmitterPosition()
	{
		if(this.transform.position.x > 5.33f)
			this.transform.position.Set(5.33f,this.transform.position.y,this.transform.position.z);
		if(transform.position.x < -5.33f)
			this.transform.position.Set(-5.33f,this.transform.position.y,this.transform.position.z);

		if(this.transform.position.y > 5)
			this.transform.position.Set(this.transform.position.x,5,this.transform.position.z);
		if(this.transform.position.y < -5)
			this.transform.position.Set(this.transform.position.x,5,this.transform.position.z);
	}
}
