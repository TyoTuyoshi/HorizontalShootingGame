using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class EditButtonController
{
    /// <summary>
    /// ボタン(パネル兼)
    /// アイコン
    /// ステータス
    /// </summary>
    private readonly Button btn_ship;
    private readonly Label lb_status;
    private readonly VisualElement img_icon;

    public EditButtonController(VisualElement visualElement)
    {
        btn_ship = visualElement.Q<Button>("btn_ship");
        //ボタンの子要素から取得
        lb_status = btn_ship.Q<Label>("lb_status");
        img_icon = btn_ship.Q<VisualElement>("img_icon");
    }

    public void SetStatus(Ship ship)
    {
        //ステータスラベルに艦船の情報を設定
        string status = $"艦名　：{ship.Name}\n";
        string[] type = { "駆逐艦", "軽巡艦", "重巡艦", "戦艦", "空母艦", "軽空母艦" };
        status += $"艦種　：{type[(int)ship.ShipType]}\n";
        status += $"耐久値：{ship.Durable}\n";
        status += $"火力　：{ship.Power}\n";
        status += $"装填　：{ship.Charge}";
       
        lb_status.text = status;

        //アイコン画像
        //インスタンスがないためGetComponent
        SpriteRenderer renderer = ship.GetComponent<SpriteRenderer>();
        img_icon.style.backgroundImage = new StyleBackground(renderer.sprite);
        img_icon.style.unityBackgroundImageTintColor = renderer.color;

        //艦船追加のアクション
        btn_ship.clicked += () =>
        {
            int index = EditSceneManager.Instance.index;

            //追加
            //まだ登録されていなければ追加
            if (EditSceneManager.Instance.AddButtons[index].stock_ship == null)
            {
                EditSceneManager.Instance.ships.Add(ship);
            }
            else
            {
                //既に登録されている場合は入れ替える。
                EditSceneManager.Instance.ships.Remove(EditSceneManager.Instance.AddButtons[index].stock_ship);
                EditSceneManager.Instance.ships.Add(ship);
            }
            
            //アクセス元のボタンの背景等を変更
            Debug.Log(index);
            var asb = EditSceneManager.Instance.AddButtons[index];
            asb.btn.text = ship.Name;
            asb.btn.style.backgroundImage = new StyleBackground(renderer.sprite);
            asb.btn.style.unityBackgroundImageTintColor = renderer.color;
          
            //追加した艦船データを保存しておく
            asb.stock_ship = ship;
            
            //追加後、選択パネルを非表示にする。
            EditSceneManager.Instance.ShipViewer.SetActive(false);
        };
    }
}
