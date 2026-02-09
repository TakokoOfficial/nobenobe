using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SoundEffectLoader : MonoBehaviour, ICommandExecutor
{
    [SerializeField]
    [Header("背景設定")]    
    public List<SoundEffect> soundEffects;

    // AudioSourceプールを用意してGCを回避
    [SerializeField]
    private int poolSize = 10;
    private List<AudioSource> audioSources;
    private Dictionary<string, float> lastPlayTime;
    private Dictionary<AudioSource, string> playingSE;

    void Awake()
    {
        // poolSize個だけリストとディクショナリとaudioSourceを用意する
        audioSources = new List<AudioSource>(poolSize);
        lastPlayTime = new Dictionary<string, float>();
        playingSE = new Dictionary<AudioSource, string>();

        for(int i = 0; i < poolSize; i++)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            audioSources.Add(src);
        }
    }

    // 再生していないAudioSourceを返す
    AudioSource GetAudioSource()
    {
        return audioSources.FirstOrDefault(s => !s.isPlaying);
    }

    // クールダウンを判定する
    bool CanPlay(SoundEffect se)
    {
        if(!lastPlayTime.TryGetValue(se.name, out float time))
            return true;

        return Time.time - time >= se.interval;
    }

    public bool CanExecute(string command)
    {
        return command == "SE";
    }

    public IEnumerator Execute(CsvRow row)
    {
        string seName = row.args[0];

        // stop指定
        if(row.args.Count >= 2 && row.args[1] == "stop")
        {
            foreach(var pair in playingSE.ToList())
            {
                if(pair.Value == seName)
                {
                    pair.Key.Stop();
                    playingSE.Remove(pair.Key);
                }
            }
            yield break;
        }

        // fadeout指定
        if(row.args.Count >= 3 && row.args[1] == "fadeout")
        {
            if(float.TryParse(row.args[2], out float duration))
            {
                foreach(var pair in playingSE.ToList())
                {
                    if(pair.Value == seName)
                    {
                        StartCoroutine(FadeOut(pair.Key, duration));
                    }
                }
            }
            yield break;
        }

        // スクリプトの引数と同じ名前の音を再生
        SoundEffect se = soundEffects.FirstOrDefault(s => s.name == seName);
        if(se == null)
        {
            Debug.LogWarning($"SE not found: {seName}");
            yield break;
        }

        // intervalチェック
        if(!CanPlay(se))
            yield break;

        // 同時再生上限
        AudioSource src = GetAudioSource();
        if(src == null)
            yield break;

        // 音量設定
        src.volume = se.volume;
        src.PlayOneShot(se.audioClip);
        playingSE[src] = se.name;
        lastPlayTime[se.name] = Time.time;

        yield break;
    }


    IEnumerator FadeOut(AudioSource src, float duration)
    {
        float startVolume = src.volume;
        float t = 0f;

        while(t < duration)
        {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        src.Stop();
        src.volume = startVolume;
        playingSE.Remove(src);
    }

    void Update()
    {
        foreach(var pair in playingSE.ToList())
        {
            if(!pair.Key.isPlaying)
            {
                playingSE.Remove(pair.Key);
            }
        }
    }
}