using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationsListButton : MonoBehaviour
{
    [SerializeField]
    public Text myText;

    public GameManager theGameManager;

    public string myTextString;

    private void Awake()
    {
        theGameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void SetText(string textString)
    {
        myTextString = textString;
        myText.text = textString;
    }

    public void OnClick()
    {
        theGameManager.OpenStationInfoMenu(myTextString);
    }
}
