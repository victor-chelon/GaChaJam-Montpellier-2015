using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SoundManager : MonoBehaviour
{
    #region Members
    [SerializeField]private List<AudioClip> _clips = new List<AudioClip>();
    [SerializeField]private List<AudioClip> _clipsLoop = new List<AudioClip>();
    [Space(10)]
    [SerializeField] private List<AudioSource> _sources = new List<AudioSource>();

    private Coroutine _coroutine;
    #endregion

    void Awake()
    {
        SoundManagerEvent.onEvent += Effect;
    }

    void OnDestroy()
    {
        SoundManagerEvent.onEvent -= Effect;
    }

    IEnumerator Callback(int index, float time)
    {
        yield return new WaitForSeconds(time);

        _sources[0].Stop();
        _sources[0].clip = _clipsLoop[index];
        _sources[0].Play();
    }

    void Effect(int index)
    {
        if (index < 6)
        {
            _sources[0].Stop();
            _sources[0].clip = _clips[index];
            _sources[0].Play();

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            if (_clips[index] != null)
                _coroutine = StartCoroutine(Callback(index , _clips[index].length));

            
            return;
        }

		_sources[index - 5].Stop();
        _sources[index - 5].clip = _clips[index];
        _sources[index - 5].Play();
    }

    public void Volume(int index, float vol)
    {
        _sources[index].volume = vol;
    }
}