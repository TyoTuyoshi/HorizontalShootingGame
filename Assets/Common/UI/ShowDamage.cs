using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ShowDamage : MonoBehaviour
{
    [System.NonSerialized] public float damage = 100;
    private TextMeshPro damage_mesh;

    private GameObject damage_obj;
    void Start()
    {
        damage_obj = this.gameObject;
        damage_mesh = damage_obj.GetComponent<TextMeshPro>();
        damage_mesh.text = $"{damage}";
    }

    private void OnEnable()
    {
        StartCoroutine(ObjectOFF());
    }

    private IEnumerator ObjectOFF()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        damage_obj.SetActive(false);
    }
}
