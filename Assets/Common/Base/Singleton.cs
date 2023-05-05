using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///各マネージャのシングルトン用クラス
/// https://kyoro-s.com/unity-7/
/// 上記のリンクから実装内容を参考
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type type = typeof(T);
                instance = (T)FindObjectOfType(type);
                if (instance == null)
                {
                    Debug.LogError(type + " をアタッチしているGameObjectが存在しません");
                }
            }

            return instance;
        }
    }

    virtual protected void Awake()
    {
        // 他のゲームオブジェクトにアタッチされているか調べる
        // アタッチされている場合は破棄する。
        CheckInstance();
    }

    protected void CheckInstance()
    {
        //インスタンスが存在していなければ、自分自身をインスタンス化
        if (instance == null)
        {
            instance = this as T;
            return;
        }

        //自分自身なら何もしない
        else if (Instance == this)
        {
            return;
        }

        //自分自身を破棄する
        Destroy(this);
        return;
    }
}
