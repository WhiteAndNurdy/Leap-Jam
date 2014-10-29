using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

    public GameObject localPlayer;
    public GameObject player2;

    private Base_Choice choicePlayer1;
    private Base_Choice choicePlayer2;

    // Use this for initialization
    void Start () {
        choicePlayer1 = localPlayer.GetComponent<Base_Choice>();
        choicePlayer2 = player2.GetComponent<Base_Choice>();
        DebugUtils.Assert(choicePlayer1 != null && choicePlayer2 != null);
    }
    
    // Update is called once per frame
    void Update () {
        if (choicePlayer1.ChoiceMade && choicePlayer2.ChoiceMade)
        {
            CalculateWinner(choicePlayer1.Draw(), choicePlayer2.Draw());
            DisplayWinner();
            choicePlayer1.Reset();
            choicePlayer2.Reset();
        }
    }

    void CalculateWinner (Choice choice1, Choice choice2)
    {
        Debug.Log("Local Player choice: " + choice1 + "(int value: " + (int)choice1 +" )");
        Debug.Log("Remote Player choice: " + choice2 + "(int value: " + (int)choice2 + " )");
        WinStatus winStatusPlayer1;
        WinStatus winStatusPlayer2;
        // check for draw
        if (choice1 == choice2)
        {
            winStatusPlayer1 = WinStatus.Draw;
            winStatusPlayer2 = WinStatus.Draw;
        }
        // Check if player 1 is victorious
        else if ((int)choice2 == MathExtensions.mod(((int)choice1 - 1), (int)Choice.Count))
        {
            winStatusPlayer1 = WinStatus.Winner;
            winStatusPlayer2 = WinStatus.Loser;
        }
        else
        {
            winStatusPlayer1 = WinStatus.Loser;
            winStatusPlayer2 = WinStatus.Winner;
        }
        localPlayer.GetComponent<Status>().PlayerStatus = winStatusPlayer1;
        player2.GetComponent<Status>().PlayerStatus = winStatusPlayer2;
    }

    void DisplayWinner()
    {
        HUD.Instance.ShowWinnerText(localPlayer.GetComponent<Status>().PlayerStatus);
    }
}
