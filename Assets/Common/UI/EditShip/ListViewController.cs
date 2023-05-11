using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class ListViewController 
{
    //private IReadOnlyList<Ship> ships;
    private readonly VisualTreeAsset _elementTemplate;
    private readonly ListView ls_ships;

    public ListViewController(VisualElement root, VisualTreeAsset elementTemplate, List<Ship> ships)
    {
        ls_ships = root.Q<ListView>("ls_ships");
        _elementTemplate = elementTemplate;

        ls_ships.makeItem = () =>
        {
            // リスト要素をインスタンス化して返す
            var element = _elementTemplate.Instantiate();
            var buttonController = new EditButtonController(element);//,new Ship());
            element.userData = buttonController; // ControllerはuserDataという汎用データ受け渡し用フィールドに格納しておく
            return element;
        };

        ls_ships.bindItem = (item, index) =>
        {
            // リスト要素にデータを設定する
            var controller = (EditButtonController)item.userData;
            controller.SetStatus(ships[index]);
        };

        ls_ships.fixedItemHeight = 240;
        ls_ships.itemsSource = ships;
    }
}
