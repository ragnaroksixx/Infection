using UnityEngine;
using System.Collections;
using DG.Tweening;
public class AudioManager : MonoBehaviour
{
    public AudioSource bgm;
    public AudioSource sfxPrefab;
    static AudioManager Instance;
    Pool<AudioSource> audioPool;
    public AudioClip defaulBGM;
    private void Awake()
    {
        Instance = this;
        audioPool = new Pool<AudioSource>(sfxPrefab.gameObject, 20);
    }
    public static void SetBgm(AudioClip ac)
    {
        if (ac == null)
            ac = Instance.defaulBGM;
        if (ac == Instance.bgm.clip) return;
        Instance.bgm.clip = ac;
        Instance.bgm.Play();
    }
    public static void Play(AudioClip ac, Vector3 pos)
    {
        if (ac == null) return;
        AudioSource a = Instance.audioPool.Pop();
        a.transform.position = pos;
        a.clip = ac;
        a.Play();
        Instance.StartCoroutine(Instance.ReturnOnStop(a));
    }

    IEnumerator ReturnOnStop(AudioSource a)
    {
        while (a.isPlaying)
        {
            yield return null;
        }
        a.Stop();
        Instance.audioPool.Push(a);
    }
}
