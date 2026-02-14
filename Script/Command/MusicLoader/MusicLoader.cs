using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MusicLoader : MonoBehaviour, ICommandExecutor
{
    [SerializeField, Header("音楽設定")]
    public List<Music> musics;

    [SerializeField, Header("AudioSourceのプレハブ")]
    public AudioSource audioSourcePrefab;

    [SerializeField, Header("マスター音量")]
    public float masterVolume = 1.0f;

    // 再生中の曲名 → AudioSource
    Dictionary<string, AudioSource> playingSources = new Dictionary<string, AudioSource>();

    // フェード管理（AudioSourceごと）
    Dictionary<AudioSource, Coroutine> volumeFades = new Dictionary<AudioSource, Coroutine>();
    Dictionary<AudioSource, Coroutine> pitchFades  = new Dictionary<AudioSource, Coroutine>();

    public bool CanExecute(string command)
    {
        return command == "BGM";
    }

    public IEnumerator Execute(CsvRow row)
    {
        if (row.args.Count < 2)
        {
            Debug.LogError("BGM 引数不足");
            yield break;
        }

        string target = row.args[0];
        string action = row.args[1];

        // play
        if (action == "play")
        {
            PlayMusic(target);
            yield break;
        }

        // stop
        if (action == "stop")
        {
            StopMusic(target);
            yield break;
        }

        // volume / pitch
        if (row.args.Count < 3)
        {
            Debug.LogError("BGM value不足: " + string.Join(",", row.args));
            yield break;
        }

        float value = float.Parse(row.args[2]);
        float duration = row.args.Count >= 4 ? float.Parse(row.args[3]) : 0f;

        foreach (var src in GetTargetSources(target))
        {
            if (action == "volume")
            {
                float targetVolume = value * masterVolume;

                if (duration <= 0f)
                {
                    src.volume = targetVolume;
                }
                else
                {
                    StartVolumeFade(src, targetVolume, duration);
                }
            }
            else if (action == "pitch")
            {
                if (duration <= 0f)
                {
                    src.pitch = value;
                }
                else
                {
                    StartPitchFade(src, value, duration);
                }
            }
        }
    }

    // 再生

    void PlayMusic(string target)
    {
        if (target == "all")
        {
            foreach (var music in musics)
                PlaySingle(music);
        }
        else
        {
            var music = musics.FirstOrDefault(m => m.name == target);
            if (music != null)
                PlaySingle(music);
        }
    }

    void PlaySingle(Music music)
    {
        if (playingSources.ContainsKey(music.name))
            return;

        var src = Instantiate(audioSourcePrefab, transform);
        src.clip = music.audioClip;
        src.volume = music.volume * masterVolume;
        src.loop = true;
        src.Play();

        playingSources[music.name] = src;
    }

    // stop

    void StopMusic(string target)
    {
        if (target == "all")
        {
            foreach (var src in playingSources.Values)
            {
                src.Stop();
                Destroy(src.gameObject);
            }
            playingSources.Clear();
            return;
        }

        if (playingSources.TryGetValue(target, out var source))
        {
            source.Stop();
            Destroy(source.gameObject);
            playingSources.Remove(target);
        }
    }

    // フェード制御

    void StartVolumeFade(AudioSource src, float target, float time)
    {
        if (volumeFades.TryGetValue(src, out var c))
            StopCoroutine(c);

        volumeFades[src] = StartCoroutine(FadeVolume(src, target, time));
    }

    void StartPitchFade(AudioSource src, float target, float time)
    {
        if (pitchFades.TryGetValue(src, out var c))
            StopCoroutine(c);

        pitchFades[src] = StartCoroutine(FadePitch(src, target, time));
    }

    IEnumerator FadeVolume(AudioSource src, float target, float time)
    {
        float start = src.volume;
        float t = 0f;

        while (t < time)
        {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(start, target, t / time);
            yield return null;
        }

        src.volume = target;
        volumeFades.Remove(src);
    }

    IEnumerator FadePitch(AudioSource src, float target, float time)
    {
        float start = src.pitch;
        float t = 0f;

        while (t < time)
        {
            t += Time.deltaTime;
            src.pitch = Mathf.Lerp(start, target, t / time);
            yield return null;
        }

        src.pitch = target;
        pitchFades.Remove(src);
    }

    // 共通

    IEnumerable<AudioSource> GetTargetSources(string target)
    {
        if (target == "all")
            return playingSources.Values;

        if (playingSources.TryGetValue(target, out var src))
            return new[] { src };

        return Enumerable.Empty<AudioSource>();
    }

    public void masterVolumeChange(float volume)
    {
        masterVolume = volume;

        foreach (var pair in playingSources)
        {
            var music = musics.FirstOrDefault(m => m.name == pair.Key);
            if (music != null)
                pair.Value.volume = music.volume * masterVolume;
        }
    }
}