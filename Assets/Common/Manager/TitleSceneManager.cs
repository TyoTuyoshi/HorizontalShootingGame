using System.Collections;
using System.Collections.Generic;
using Manager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleSceneManager : Singleton<MonoBehaviour>
{
    [SerializeField] private UIDocument uiDocument;
    // Start is called before the first frame update
    void Start()
    {
        var root = uiDocument.rootVisualElement;
        //編隊シーン遷移ボタン
        Button btn_edit = root.Query<Button>("btn_edit");
        
        //シーンのフェードイン/アウト用Imageの設定
        FadeManager.Instance.SetFadeImage(true);
        //遷移時間
        const float fade_time = 0.5f;
        FadeManager.Instance.SceneObj.SceneFadeOUT(fade_time);
        //フェードアウト後にfalse
        Invoke("SetFadeImage",fade_time);
        
        //編隊シーンへ遷移するアクション
        btn_edit.clicked += () =>
        {
            FadeManager.Instance.FadeImage.gameObject.SetActive(true);
            FadeManager.Instance.SceneObj.SceneFadeIN("EditScene",1.0f);
        };

        //ゲーム終了ボタン
        Button btn_append = root.Query<Button>("btn_append");
        btn_append.clicked += () =>
        {
            Debug.Log("END");
            Application.Quit();
        };
    }
    
    /// <summary>
    ///Invoke()呼び出し用ラッパ
    /// </summary>
    private void SetFadeImage()
    {
        FadeManager.Instance.SetFadeImage(false);
    }
}
