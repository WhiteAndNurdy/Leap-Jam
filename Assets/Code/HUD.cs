using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {


    // singleton
    private static HUD mInstance;

    public GUIStyle WinnerTextStyle;
    public Rect WinnerTextRect;
    public string WinnerTextString;

    private bool showWinnerText = false;

    public static HUD Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = GameObject.FindObjectOfType<HUD>();
            }
            return mInstance;
        }
    }

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    void OnGUI ()
    {
        if (showWinnerText)
        {
            Rect centeredRect = new Rect(
                (Screen.width - WinnerTextRect.width) / 2,
                (Screen.height - WinnerTextRect.height) / 2,
                WinnerTextRect.width,
                WinnerTextRect.height
                );
            GUI.TextArea(centeredRect, WinnerTextString, WinnerTextStyle);
        }
    }

    public void ShowWinnerText( WinStatus status )
    {
        if (status == WinStatus.Winner)
            WinnerTextString = "You Won!";
        else if (status == WinStatus.Loser)
            WinnerTextString = "You Lost!";
        else if (status == WinStatus.Draw)
            WinnerTextString = "It's a draw...";
        showWinnerText = true;
    }

    public void HideWinnerText()
    {
        showWinnerText = false;
    }
}
