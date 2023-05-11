using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipListView : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset _elementTemplate;

    private void OnEnable()
    {
        // UIDocumentコンポーネントについては次節で説明
        var root = GetComponent<UIDocument>().rootVisualElement;
        //var buttonTexts = new List<string> {"First", "Second", "Third"};

        var db_ships = ShipDataBase.Instance.ShipDB;
        var set_ships = EditSceneManager.Instance.ships;

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
            
        };

        //データベースにある艦船のうち、未だ設定されていないもの
        List<Ship> get_ships = db_ships.Except(set_ships).ToList();
        
        new ListViewController(root, _elementTemplate, get_ships);
    }
}
