using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AnimationLoader : MonoBehaviour, ICommandExecutor
{
    [SerializeField]
    [Header("アニメーション設定")]
    public List<Animation> animations;

    public bool CanExecute(string command)
    {
        return command == "Animation";
    }
    public IEnumerator Execute(CsvRow row)
    {
        // スクリプトの引数と同じ名前のアニメーションを再生
        Animation matchedAnimation = animations.FirstOrDefault(a => a.name == row.args[0]);
        Debug.Log("" + matchedAnimation.name);
        matchedAnimation.animator.Play(matchedAnimation.stateName);

        yield break;
    }
}
