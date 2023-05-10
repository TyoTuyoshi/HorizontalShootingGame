using UnityEngine;

/// <summary>
/// https://umistudioblog.com/singletonhowto/
/// 上のリンクからクラスの実装を引用
/// </summary>
/// <typeparam name="T">シーン間でデータを共有したいもの</typeparam>
public class SingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)FindObjectOfType(typeof(T));
            DontDestroyOnLoad(gameObject);
        }   
        else Destroy(gameObject);
    }
}