using UnityEngine;
using System.Collections;
using TMPro;

public class CountUp : MonoBehaviour
{
    [Header("数字をカウントアップする速度（秒）")]
    [SerializeField]
    public float speed = 1f;

    // スコアを表示するテキストの数字をなめらかに増減させる
    public void CountUpStart(TextMeshProUGUI textMeshProUGUI, int end)
    {
        int nowCount = 0;

        if (int.TryParse(textMeshProUGUI.text, out nowCount))
        {
            StartCoroutine(CountUpCoroutine(textMeshProUGUI, nowCount, end));
        }
        else
        {
            // 変換失敗時は0スタート
            StartCoroutine(CountUpCoroutine(textMeshProUGUI, 0, end));
        }
    }

    private IEnumerator CountUpCoroutine(TextMeshProUGUI text, int start, int end)
    {
        float time = 0f;

        while (time < speed)
        {
            time += Time.deltaTime;
            float t = time / speed;

            // なめらかに補間
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(start, end, t));

            text.text = currentValue.ToString();

            yield return null;
        }

        // 最後は必ずend値にする
        text.text = end.ToString();
    }
}