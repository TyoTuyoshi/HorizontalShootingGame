using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    /// <summary>
    /// MonoBehaviourを継承しないとコールチンを呼べなくなってるので、
    /// GameManagerでインスタンスを作っておく
    /// 呼び出しはGameManagerから
    /// </summary>
    public class Scene : MonoBehaviour
    {
        /// <summary>
        /// シーン遷移のフェードインアウトの基底
        /// </summary>
        /// <param name="name">Scene名</param>
        /// <param name="wait">ロードまでの待ち時間</param>
        /// <returns></returns>
        private IEnumerator BaseFade(float wait = 0.1f, bool is_fadein = true, string name = "")
        {
            yield return null;

            float time = 0.0f;
            Color color = Color.black;
            //フェードイン　　(a:0->1)
            //フェードアウト　(a:1->0)
            color.a = is_fadein ? 0 : 1;
            while (time <= wait)
            {
                time += Time.deltaTime;
                //フェードイン
                if (is_fadein) color.a = (time / wait);
                //フェードアウト
                else color.a = 1.0f - (time / wait);

                FadeManager.Instance.FadeImage.color = color;
                //Debug.Log(time);
                yield return null;
            }
            
            Debug.Log($"Scene=>{time}");
            if (is_fadein) UnityEngine.SceneManagement.SceneManager.LoadScene(name);
        }

        public void SceneFadeIN(string name, float wait = 0.1f)
        {
            StartCoroutine(BaseFade(wait, true, name));
        }

        public void SceneFadeOUT(float wait)
        {
            StartCoroutine(BaseFade(wait, false));
        }
    }
}