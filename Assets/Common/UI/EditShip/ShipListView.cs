using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipListView : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset vtree_elements;

    private void OnEnable()
    {
        // UIDocumentコンポーネントについては次節で説明
        var root = GetComponent<UIDocument>().rootVisualElement;
        //var buttonTexts = new List<string> {"First", "Second", "Third"};

        //戻るボタン(アクティブをfalse)
        Button btn_back = root.Q<Button>("btn_back");
        btn_back.clicked += () =>
        {
            this.gameObject.SetActive(false);
        };
        
        //除隊ボタン
        Button btn_remove = root.Q<Button>("btn_remove");
        
        btn_remove.clicked += () =>
        {
            //アクセス元の情報をクリア
            int index = EditSceneManager.Instance.index;
            Debug.Log(index);

            //追加ボタン
            var asb = EditSceneManager.Instance.AddButtons[index];
            //アクセス元の艦船を除隊
            if (asb.ship != null)
            {
                //var ship = EditSceneManager.Instance.ships[index];
                //艦船の除隊
                EditSceneManager.Instance.ships.Remove(asb.ship);
                //ボタンの背景などをクリア
                //var asb = EditSceneManager.Instance.AddButtons[index];
                asb.btn.text = $"追加{index + 1}";
                asb.btn.style.backgroundImage = null;
                asb.btn.style.unityBackgroundImageTintColor = Color.white;
            }

            //除隊後、選択パネルを非表示にする。
            EditSceneManager.Instance.ShipViewer.SetActive(false);
        };

        var db_ships = ShipDataBase.Instance.ShipDB;
        var set_ships = EditSceneManager.Instance.ships;
        //var gm_ships = GameManager.Instance.PlayAbleShip;
        //データベースにある艦船のうち、未だ設定されていないもの
        List<Ship> get_ships = db_ships.Except(set_ships).ToList();
        
        new ListViewController(root, vtree_elements, get_ships);
    }
}
