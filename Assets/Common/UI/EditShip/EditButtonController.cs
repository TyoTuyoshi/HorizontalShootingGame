using System;
using System.Collections;
using System.Collections.Generic;
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

    public EditButtonController(VisualElement visualElement)//,Ship ship)
    {
        btn_ship = visualElement.Q<Button>("btn_ship");
        //ボタンの子要素から取得
        lb_status = btn_ship.Q<Label>("lb_status");
        img_icon = btn_ship.Q<VisualElement>("img_icon");

        //ステータスラベルに艦船の情報を設定
        lb_status.text = "des";
        btn_ship.clicked += OnClick;
    }

    public void SetStatus(Ship ship)
    {
        //ステータスラベルに艦船の情報を設定

        string status = $"艦名　：{ship.Name}\n";
        status += "艦種　：";
        switch (ship.ShipType)
        {
            case Ship.Type.Destroyer:
                status += $"駆逐艦\n";
                break;
            case Ship.Type.LCruiser:
                status += $"軽巡艦\n";
                break;
            case Ship.Type.HCruiser:
                status += $"重巡艦\n";
                break;
            //以下未実装のタイプ
            case Ship.Type.AirCarrier:
                status += $"空母艦\n";
                break;
            case Ship.Type.LAirCarrier:
                status += $"軽空母艦\n";
                break;
            case Ship.Type.BattleShip:
                status += $"戦艦\n";
                break;
        }
        status += $"耐久値：{ship.Durable}\n";
        status += $"火力　：{ship.Power}\n";
        status += $"装填　：{ship.Charge}";
       
        lb_status.text = status;

        //アイコン画像
        //インスタンスがないためGetComponent
        SpriteRenderer renderer = ship.GetComponent<SpriteRenderer>();
        img_icon.style.backgroundImage = new StyleBackground(renderer.sprite);
        img_icon.style.unityBackgroundImageTintColor = renderer.color;
    }

    /// <summary>
    /// フラグ管理用
    /// </summary>
    private void OnClick()
    {
        Debug.Log("Clicked: Select");
    }
}
