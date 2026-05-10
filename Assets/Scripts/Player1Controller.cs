using UnityEngine;

public class Player1Controller : MonoBehaviour
{
    public void Player1Scores()
    {
        GameManager.Instance.Player1Scores();
    }

    public void Player2Scores()
    {
        GameManager.Instance.Player2Scores();
    }
}