using System.Collections;
using TMPro;
using UnityEngine;

public class ShowDamage : MonoBehaviour
{
    //SetActive()をShipから呼び出す。
    private void OnEnable()
    {
        this.gameObject.GetComponent<MeshRenderer>().sortingLayerName = "UI";
        StartCoroutine(ActiveOFF(0.15f));
    }

    /// <summary>
    /// 一定時間後に非表示
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActiveOFF(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        gameObject.GetComponent<TextMeshPro>().text = "";
        gameObject.SetActive(false);
    }
}