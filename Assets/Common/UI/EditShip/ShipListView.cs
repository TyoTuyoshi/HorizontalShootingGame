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
            //アクセス元の艦船を除隊
            /*
            if (EditSceneManager.Instance.ships.Count != 0 &&
                EditSceneManager.Instance.AddButtons[index].text != $"追加{index + 1}")
            {
                var ship = EditSceneManager.Instance.ships[index];
                EditSceneManager.Instance.ships.Remove(ship);
                //ボタンの背景などをクリア
                var btn = EditSceneManager.Instance.AddButtons[index];
                btn.text = $"追加{index + 1}";
                btn.style.backgroundImage = null;
                btn.style.unityBackgroundImageTintColor = Color.white;
            }*/

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
