using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipListView : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset _elementTemplate;

    private void OnEnable()
    {
        // UIDocumentコンポーネントについては次節で説明
        var uiDocument = GetComponent<UIDocument>();
        //var buttonTexts = new List<string> {"First", "Second", "Third"};
        
        
        new ListViewController(uiDocument.rootVisualElement, _elementTemplate, ShipDataBase.Instance.ShipDB);
    }
}
