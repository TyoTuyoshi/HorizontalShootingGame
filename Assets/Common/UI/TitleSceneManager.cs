using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    private Button test;
    // Start is called before the first frame update
    void Start()
    {
        test = uiDocument.rootVisualElement.Query<Button>().First();
        test.clicked += OnButtonClick;
    }

    private void OnButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
